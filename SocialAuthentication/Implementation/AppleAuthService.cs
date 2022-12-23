using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Rapplis.SocialAuthentication.Models;

namespace Rapplis.SocialAuthentication.Implementation;

public class AppleAuth : ISocialAuth
{
    private readonly SocialAuthData _data;

    public AppleAuth(SocialAuthData data)
    {
        _data = data;
    }
    
    public async Task<SocialUser> GetUser(string token)
    {
        var claims = await VerifyAppleIdToken(token, _data.AppleClientId);
        return new SocialUser
        {
            Provider = SocialAuthProvider.Apple,
            Email = claims.FirstOrDefault(x => x.Type == "email")?.Value,
            EmailVerified = bool.Parse(claims.FirstOrDefault(x => x.Type == "email_verified")?.Value?? "false"),
            Id = claims.FirstOrDefault(x => x.Type == "sub")?.Value
        };
    }
    
    private async Task<List<Claim>> VerifyAppleIdToken(string token, string clientId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        var claims = jwtSecurityToken.Claims.ToList();
        // ReSharper disable once NotAccessedVariable
        SecurityKey? publicKey; SecurityToken validatedToken;

        var expirationClaim = claims.FirstOrDefault(x => x.Type == "exp")?.Value;
        var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim!)).DateTime;

        if (expirationTime < DateTime.UtcNow)
        {
            throw new Exception("Token expired");
        }

        using (var httpClient = new HttpClient())
        {
            var applePublicKeys = await httpClient.GetAsync("https://appleid.apple.com/auth/keys?scope=name%20email");
            var keySet = new JsonWebKeySet(applePublicKeys.Content.ReadAsStringAsync().Result);

            publicKey = keySet.Keys.FirstOrDefault(x => x.Kid == jwtSecurityToken.Header.Kid);
        }

        var validationParameters = new TokenValidationParameters
        {
            ValidIssuer = "https://appleid.apple.com",
            IssuerSigningKey = publicKey,
            ValidAudience = clientId
        };
            
        tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        return claims;
    }
}