using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Rapplis.SocialAuthentication.Models;

namespace Rapplis.SocialAuthentication.Implementation;

public class FacebookAuth : ISocialAuth
{
    private readonly SocialAuthData _data;

    public FacebookAuth(SocialAuthData data)
    {
        _data = data;
    }
    
    public async Task<SocialUser> GetUser(string token)
    {
        
        const string url = "https://graph.facebook.com/me?fields=id,name,gender,email,birthday,first_name,last_name";
            
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        
        var response = await httpClient.GetAsync(url);
        if (response.StatusCode != HttpStatusCode.OK) throw new Exception("Facebook API returned an error");
            
        var content = await response.Content.ReadAsStringAsync();
        var profile = JsonConvert.DeserializeObject<FacebookProfile>(content);
        if (profile == null) throw new Exception("Couldn't read user information");

        var socialUser = new SocialUser
        {
            Provider = SocialAuthProvider.Facebook,
            Id = profile.id,
        };

        if (!string.IsNullOrEmpty(profile.first_name)) socialUser.FirstName = profile.first_name;
        if (!string.IsNullOrEmpty(profile.last_name)) socialUser.LastName = profile.last_name;
        if (!string.IsNullOrEmpty(profile.email))
        {
            socialUser.Email = profile.email;
            socialUser.EmailVerified = true;
        }

        if (!string.IsNullOrEmpty(profile.gender))
            socialUser.Gender = char.ToUpper(profile.gender.First()) + profile.gender[1..].ToLower();
        if(!string.IsNullOrEmpty(profile.birthday))
            socialUser.Birthday = DateTime.Parse(profile.birthday);
            
        return socialUser;
    }

    private class FacebookProfile
    {
        public string id { get; set; }
        public string name { get; set; }
        // ReSharper disable once InconsistentNaming
        public string gender { get; set; }
        public string email { get; set; }
        public string birthday { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
}