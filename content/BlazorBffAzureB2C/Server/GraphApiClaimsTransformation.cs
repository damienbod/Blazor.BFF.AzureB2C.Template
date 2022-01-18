using BlazorBffAzureB2C.Server.Services;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorBffAzureB2C.Server
{
    public class GraphApiClaimsTransformation : IClaimsTransformation
    {
        private readonly GraphApiClientService _graphApiClientService;

        public GraphApiClaimsTransformation(GraphApiClientService graphApiClientService)
        {

            _graphApiClientService = graphApiClientService;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            var groupClaimType = "group";
            if (!principal.HasClaim(claim => claim.Type == groupClaimType))
            {
                var nameidentifierClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
                var nameidentifier = principal.Claims.FirstOrDefault(t => t.Type == nameidentifierClaimType);

                var groupIds = await _graphApiClientService.GetGraphApiUserMemberGroups(nameidentifier.Value);

                foreach (var groupId in groupIds.ToList())
                {
                    claimsIdentity.AddClaim(new Claim(groupClaimType, groupId));
                }
            }

            principal.AddIdentity(claimsIdentity);
            return principal;
        }


    }
}
