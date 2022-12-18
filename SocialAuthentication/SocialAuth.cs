using Rapplis.SocialAuthentication.Implementation;
using Rapplis.SocialAuthentication.Models;

namespace Rapplis.SocialAuthentication;

public abstract class SocialAuth
{
    public static async Task<SocialUser> GetUser(SocialAuthData data)
    {
        if (!IsValidProvider(data.Provider))
            throw new ArgumentException("Invalid provider");
        
        var socialAuthServiceBuilder = new SocialAuthBuilder();
        var socialAuthService = socialAuthServiceBuilder.GetService(data);
        return await socialAuthService.GetUser(data.Token);
    }
    
    private static bool IsValidProvider(string provider)
    {
        return SocialAuthProvider.Values.Any(x => provider.ToLower().Equals(x));
    }
}