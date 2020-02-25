using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
        private readonly IMicrosoftGraphRepository microsoftGraphRepository;
        private readonly IDataRepository<Thing> thingRepository;

        public AuthorizationRepository(IMicrosoftGraphRepository microsoftGraphRepository, IDataRepository<Thing> thingRepository)
        {
            this.microsoftGraphRepository = microsoftGraphRepository ?? throw new ArgumentNullException(nameof(microsoftGraphRepository));
            this.thingRepository = thingRepository ?? throw new ArgumentNullException(nameof(thingRepository));
        }

        public async Task<IEnumerable<PolicyResult>> EvaluateAsync(AuthenticationHeaderValue authenticationHeaderValue, InitialState<Group> initialState)
        {
            _ = initialState ?? throw new ArgumentNullException(nameof(initialState));

            var user = await microsoftGraphRepository.FetchMeAsync(authenticationHeaderValue);
            var things = await thingRepository.FindAsync(t => t.Identifier == initialState.Identifier);

            var expressionContext = new ExpressionContext();
            expressionContext.Imports.AddType(typeof(ListParser));

            var variables = expressionContext.Variables;
            variables.Add("userPrincipalName", initialState.ClaimsPrincipal.Identity.Name);
            variables.Add("groups", initialState.Groups);

            var policyResults = new List<PolicyResult>();
            if (user != null)
            {
                if (!string.IsNullOrWhiteSpace(user.OfficeLocation)) variables.Add($"user_location", user.OfficeLocation);

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
