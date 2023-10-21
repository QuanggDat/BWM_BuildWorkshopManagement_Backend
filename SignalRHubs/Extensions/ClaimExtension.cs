using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SignalRHubs.Extensions
{
    public static class ClaimExtension
    {
        public static List<string> GetRoles(this ClaimsPrincipal user)
        {
            var x = user.HasClaim(x => x.Type == ClaimTypes.Role);
            var claims = user.Claims.Where(x => x.Type == ClaimTypes.Role).ToList();
            if (claims.Any())
            {
                return claims.Select(x => x.Value).ToList();
            }
            throw new Exception("Invalid token");
        }

        public static string GetId(this ClaimsPrincipal user)
        {
            var claimType = "UserId";
            var idClaim = user.Claims.FirstOrDefault(x => x.Type == claimType);
            if (idClaim != null)
            {
                return new Guid(idClaim.Value).ToString();
            }
            throw new Exception("Invalid token");
        }
    }
}
