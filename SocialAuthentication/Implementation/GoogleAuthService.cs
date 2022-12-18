using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Rapplis.SocialAuthentication.Models;

namespace Rapplis.SocialAuthentication.Implementation;

public class GoogleAuth : ISocialAuth
{
    private readonly SocialAuthData _data;

    public GoogleAuth(SocialAuthData data)
    {
        _data = data;
    }
    
    public async Task<SocialUser> GetUser(string token)
    {
        const string url = "https://people.googleapis.com/v1/people/me?personFields=addresses,birthdays,genders,names,emailAddresses";
            
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        
        var response = await httpClient.GetAsync(url);
        if (response.StatusCode != HttpStatusCode.OK) throw new Exception("Google API returned an error");
            
        var content = await response.Content.ReadAsStringAsync();
        var profile = JsonConvert.DeserializeObject<GoogleProfile>(content);
        if (profile == null) throw new Exception("Error reading Google profile");

        var socialUser = new SocialUser
        {
            Provider = SocialAuthProvider.Google,
            Id = profile.resourceName.Replace("people/", ""),
        };

        if (profile.names is { Count: > 0 })
        {
            socialUser.Name = profile.names[0].displayName;
            socialUser.FirstName = profile.names[0].givenName;
            socialUser.LastName = profile.names[0].familyName;
        }
        if (profile.emailAddresses is { Count: > 0 })
        {
            var primaryEmail = profile.emailAddresses.FirstOrDefault(e => e.metadata.primary);
            socialUser.Email = primaryEmail?.value;
            socialUser.EmailVerified = primaryEmail?.metadata.verified??false;
        }
        if(profile.photos is { Count: > 0 }) socialUser.Picture = profile.photos[0].url;
        if(profile.locales is { Count: > 0 }) socialUser.Locale = profile.locales[0].value;
        if (profile.birthdays is { Count: > 0 })
        {
            var birthday = profile.birthdays[0].date;
            if(birthday is { day: { }, month: { }, year: { } })
                socialUser.Birthday = new DateTime(birthday.year.Value, birthday.month.Value, birthday.day.Value);
        }
        if(profile.genders is {Count: >0}) socialUser.Gender = profile.genders[0].formattedValue;
            
        return socialUser;

    }

    private class GoogleProfile
    {
        public List<AgeRange> ageRanges { get; set; }
        public List<Birthday> birthdays { get; set; }
        public List<Photo> photos { get; set; }
        public string etag { get; set; }
        public List<Name> names { get; set; }
        public List<Gender> genders { get; set; }
        public string ageRange { get; set; }
        public string resourceName { get; set; }
        public List<EmailAddress> emailAddresses { get; set; }
        public List<CoverPhoto> coverPhotos { get; set; }
        public List<Locale> locales { get; set; }
        public Metadata metadata { get; set; }
    }
    
    private class AgeRange
    {
        public string ageRange { get; set; }
        public Metadata metadata { get; set; }
    }

    private class Birthday
    {
        public Date date { get; set; }
        public Metadata metadata { get; set; }
    }

    private class CoverPhoto
    {
        public string url { get; set; }
        public bool @default { get; set; }
        public Metadata metadata { get; set; }
    }

    private class Date
    {
        public int? month { get; set; }
        public int? day { get; set; }
        public int? year { get; set; }
    }

    private class EmailAddress
    {
        public string value { get; set; }
        public Metadata metadata { get; set; }
    }

    private class Gender
    {
        public string formattedValue { get; set; }
        public string value { get; set; }
        public Metadata metadata { get; set; }
    }

    private class Locale
    {
        public string value { get; set; }
        public Metadata metadata { get; set; }
    }

    private class Metadata
    {
        public Source source { get; set; }
        public bool primary { get; set; }
        public bool sourcePrimary { get; set; }
        public bool verified { get; set; }
        public List<Source> sources { get; set; }
        public string objectType { get; set; }
    }

    private class Name
    {
        public string displayNameLastFirst { get; set; }
        public string displayName { get; set; }
        public string familyName { get; set; }
        public string unstructuredName { get; set; }
        public string givenName { get; set; }
        public Metadata metadata { get; set; }
    }

    private class Photo
    {
        public string url { get; set; }
        public Metadata metadata { get; set; }
    }

    private class ProfileMetadata
    {
        public List<string> userTypes { get; set; }
        public string objectType { get; set; }
    }

    private class Source
    {
        public string type { get; set; }
        public string id { get; set; }
    }

    private class Source9
    {
        public DateTime updateTime { get; set; }
        public string etag { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public ProfileMetadata profileMetadata { get; set; }
    }
}