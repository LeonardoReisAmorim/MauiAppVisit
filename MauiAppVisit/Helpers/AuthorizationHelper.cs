using System.IdentityModel.Tokens.Jwt;

namespace MauiAppVisit.Helpers
{
    public static class AuthorizationHelper
    {
        public static bool HasToken()
        {
            var token = Preferences.Get("token", string.Empty);
            return !string.IsNullOrWhiteSpace(token) && IsTokenValid(token);
        }

        public static bool IsTokenValid(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(token))
                return false;

            var jwtToken = jwtHandler.ReadJwtToken(token);

            var expirationDateUnix = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

            if (expirationDateUnix != null)
            {
                var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationDateUnix)).UtcDateTime;
                if (expirationDateTime > DateTime.UtcNow)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
