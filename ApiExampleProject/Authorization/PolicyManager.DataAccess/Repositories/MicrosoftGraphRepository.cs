using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ApiExampleProject.Authentication.Interfaces;
using Microsoft.Graph;
using PolicyManager.DataAccess.Interfaces;
using Group = Microsoft.Graph.Group;

namespace PolicyManager.DataAccess.Repositories
{
    public class MicrosoftGraphRepository
        : IMicrosoftGraphRepository
    {
        private readonly ITokenCreator tokenCreator;

        public MicrosoftGraphRepository(ITokenCreator tokenCreator)
        {
            this.tokenCreator = tokenCreator ?? throw new ArgumentNullException(nameof(tokenCreator));
        }

        public async Task<IEnumerable<Group>> FetchMyGroupsAsync(AuthenticationHeaderValue authenticationHeaderValue)
        {
            _ = authenticationHeaderValue ?? throw new ArgumentNullException(nameof(authenticationHeaderValue));

            var scopes = new List<string>()
            {
                "https://graph.microsoft.com/User.Read",
                "https://graph.microsoft.com/Group.Read.All",
            };

            var delegateAuthenticationProvider = new DelegateAuthenticationProvider(
                async (requestMessage) =>
                {
                    var userToken = await tokenCreator.GetAccessTokenOnBehalfOf(authenticationHeaderValue.Parameter);

                    // Append the access token to the request.
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", userToken);
                });

            var graphServiceClient = new GraphServiceClient(delegateAuthenticationProvider);
            var userMemberOfCollection = await graphServiceClient.Me.MemberOf
                .Request()
                .GetAsync();

            var graphGroups = new List<Group>();
            foreach (DirectoryObject directoryObject in userMemberOfCollection)
            {
                if (directoryObject is Group)
                {
                    var group = directoryObject as Group;
                    graphGroups.Add(new Group()
                    {
                        Id = group.Id,
                        DisplayName = group.DisplayName,
                    });
                }

                if (directoryObject is DirectoryRole)
                {
                    var directoryRole = directoryObject as DirectoryRole;
                    graphGroups.Add(new Group()
                    {
                        Id = directoryRole.Id,
                        DisplayName = directoryRole.DisplayName,
                    });
                }
            }

            return graphGroups;
        }
    }
}
