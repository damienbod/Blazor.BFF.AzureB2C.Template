using Microsoft.Graph;

namespace BlazorBffAzureB2C.Server.Services;

public class MsGraphService
{
    private readonly GraphApplicationClientService _graphApplicationClientService;

    public MsGraphService(GraphApplicationClientService graphApplicationClientService)
    {
        _graphApplicationClientService = graphApplicationClientService;
    }

    public async Task<User> GetGraphApiUser(string userId)
    {
        var graphServiceClient = _graphApplicationClientService.GetGraphClientWithClientSecretCredential();

        return await graphServiceClient.Users[userId]
            .Request()
            .GetAsync();
    }

    public async Task<IUserAppRoleAssignmentsCollectionPage> GetGraphApiUserAppRoles(string userId)
    {
        var graphServiceClient = _graphApplicationClientService.GetGraphClientWithClientSecretCredential();

        return await graphServiceClient.Users[userId]
            .AppRoleAssignments
            .Request()
            .GetAsync();
    }

    public async Task<IDirectoryObjectGetMemberGroupsCollectionPage> GetGraphApiUserMemberGroups(string userId)
    {
        var graphServiceClient = _graphApplicationClientService.GetGraphClientWithClientSecretCredential();

        var securityEnabledOnly = true;

        return await graphServiceClient.Users[userId]
            .GetMemberGroups(securityEnabledOnly)
            .Request()
            .PostAsync();
    }
}