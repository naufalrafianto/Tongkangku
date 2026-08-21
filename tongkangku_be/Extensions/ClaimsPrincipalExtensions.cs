using System.Security.Claims;

namespace tongkangku_be.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(
                ClaimTypes.NameIdentifier
            )?.Value;

            if (
                string.IsNullOrWhiteSpace(idClaim) ||
                !Guid.TryParse(idClaim, out var userId)
            )
            {
                throw new UnauthorizedAccessException(
                    "User id claim is missing or invalid."
                );
            }

            return userId;
        }


        public static string GetUserRole(this ClaimsPrincipal user)
        {
            var roleClaim = user.FindFirst(
                ClaimTypes.Role
            )?.Value;

            if (string.IsNullOrWhiteSpace(roleClaim))
            {
                throw new UnauthorizedAccessException(
                    "User role claim is missing."
                );
            }

            return roleClaim;
        }

    }
}
