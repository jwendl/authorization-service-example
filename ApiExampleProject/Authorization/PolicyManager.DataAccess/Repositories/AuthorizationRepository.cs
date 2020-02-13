using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flee.PublicTypes;
using PolicyManager.DataAccess.Functions;
using PolicyManager.DataAccess.Interfaces;
using PolicyManager.DataAccess.Models;
using Group = Microsoft.Graph.Group;

namespace PolicyManager.DataAccess.Repositories
{
    public class AuthorizationRepository
        : IAuthorizationRepository
    {
        private readonly IDataRepository<User> userRepository;
        private readonly IDataRepository<Thing> thingRepository;

        public AuthorizationRepository(IDataRepository<User> userRepository, IDataRepository<Thing> thingRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.thingRepository = thingRepository ?? throw new ArgumentNullException(nameof(thingRepository));
        }

        public async Task<IEnumerable<PolicyResult>> EvaluateAsync(InitialState<Group> initialState)
        {
            _ = initialState ?? throw new ArgumentNullException(nameof(initialState));

            var user = await userRepository.FindSingleAndIncludeAsync(u => u.UserPrincipalName == initialState.ClaimsPrincipal.Identity.Name, u => u.UserAttributes);

            var things = await thingRepository.FindAsync(t => t.Identifier == initialState.Identifier);

            var expressionContext = new ExpressionContext();
            expressionContext.Imports.AddType(typeof(ListParser));

            var variables = expressionContext.Variables;
            variables.Add("userPrincipalName", initialState.ClaimsPrincipal.Identity.Name);
            variables.Add("groups", initialState.Groups);

            var policyResults = new List<PolicyResult>();
            if (user != null)
            {
                foreach (var keyValuePair in user.UserAttributes)
                {
                    variables.Add($"user_{keyValuePair.Key}", keyValuePair.Value);
                }

                foreach (var thing in things)
                {
                    foreach (var thingAttribute in thing.ThingAttributes)
                    {
                        variables.Add($"thing_{thingAttribute.Key}", thingAttribute.Value);
                    }
                }

                foreach (var thing in things)
                {
                    foreach (var thingPolicy in thing.ThingPolicies)
                    {
                        var compiledExpression = expressionContext.CompileGeneric<bool>(thingPolicy.Expression);
                        var result = compiledExpression.Evaluate();
                        policyResults.Add(new PolicyResult()
                        {
                            Name = thingPolicy.Name,
                            Description = thingPolicy.Description,
                            Result = result ? PolicyEvaluation.Allow : PolicyEvaluation.Deny
                        });
                    }
                }
            }

            if (!policyResults.Any())
            {
                policyResults.Add(new PolicyResult()
                {
                    Name = "None",
                    Description = "The default deny policy",
                    Result = PolicyEvaluation.Deny,
                });
            }

            return policyResults;
        }
    }
}
