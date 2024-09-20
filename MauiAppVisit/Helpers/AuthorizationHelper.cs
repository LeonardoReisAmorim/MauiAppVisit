using System.IdentityModel.Tokens.Jwt;

namespace MauiAppVisit.Helpers
{
    public static class AuthorizationHelper
    {
        public async static Task<bool> HasToken()
        {
            var token = await GetToken();
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

        public async static Task<string> GetToken()
        {
            return await SecureStorage.GetAsync("token");
        }

        public static string GetUserId()
        {
            return Preferences.Get("usuarioId", string.Empty);
        }

        public async static Task SetDataUser(string token, int usuarioId)
        {
            await SecureStorage.SetAsync("token", token);
            Preferences.Set("usuarioId", usuarioId);
        }
    }
}
