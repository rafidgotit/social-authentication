using Rapplis.SocialAuthentication.Implementation;
using Rapplis.SocialAuthentication.Models;

namespace Rapplis.SocialAuthentication.Implementation;

public interface ISocialAuth
{
    Task<SocialUser> GetUser(string token);
}

public class SocialAuthBuilder
{
    public SocialAuthBuilder() {}
    
    public ISocialAuth GetService(SocialAuthData data)
    {
        var services = new Dictionary<string, ISocialAuth>
        {
            { SocialAuthProvider.Google, new GoogleAuth(data) },
            { SocialAuthProvider.Facebook, new FacebookAuth(data) },
            { SocialAuthProvider.Apple, new AppleAuth(data) },
        };
        return services[data.Provider];
    }
}