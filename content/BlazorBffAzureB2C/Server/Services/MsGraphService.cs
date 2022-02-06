using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System.Threading.Tasks;

namespace BlazorBffAzureB2C.Server.Services
{
    public class MsGraphService
    {
        private readonly GraphServiceClient _graphServiceClient;

        public MsGraphService(IConfiguration configuration)
        {
            string[] scopes = configuration.GetValue<string>("GraphApi:Scopes")?.Split(' ');
            var tenantId = configuration.GetValue<string>("GraphApi:TenantId");

            // Values from app registration
            var clientId = configuration.GetValue<string>("GraphApi:ClientId");
            var clientSecret = configuration.GetValue<string>("GraphApi:ClientSecret");

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            // https://docs.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            _graphServiceClient = new GraphServiceClient(clientSecretCredential, scopes);
        }

        public async Task<User> GetGraphApiUser(string userId)
        {
            return await _graphServiceClient.Users[userId]
                    .Request()
                    .GetAsync()
                    .ConfigureAwait(false);
        }

        public async Task<IUserAppRoleAssignmentsCollectionPage> GetGraphApiUserAppRoles(string userId)
        {
            return await _graphServiceClient.Users[userId]
                    .AppRoleAssignments
                    .Request()
                    .GetAsync()
                    .ConfigureAwait(false);
        }

        public async Task<IDirectoryObjectGetMemberGroupsCollectionPage> GetGraphApiUserMemberGroups(string userId)
        {
            var securityEnabledOnly = true;

            return await _graphServiceClient.Users[userId]
                .GetMemberGroups(securityEnabledOnly)
                .Request().PostAsync()
                .ConfigureAwait(false);
        }
    }
}

