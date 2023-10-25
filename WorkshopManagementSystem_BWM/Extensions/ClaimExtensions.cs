using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace WorkshopManagementSystem_BWM.Extensions
{
    public static class ClaimExtensions
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

        public static Guid GetId(this ClaimsPrincipal user)
        {
            var claimType = "UserId";
            var idClaim = user.Claims.FirstOrDefault(x => x.Type == claimType);
            if (idClaim != null)
            {
                return new Guid(idClaim.Value);
            }
            throw new Exception("Invalid token");
        }
    }
}
