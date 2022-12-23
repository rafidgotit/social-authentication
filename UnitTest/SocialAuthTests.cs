using Rapplis.SocialAuthentication.Models;
using Xunit.Abstractions;

namespace UnitTest
{
    public class SocialAuthTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public SocialAuthTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task TestGoogle()
        {
            var googleAuthData = new SocialAuthData
            {
                Token = "",
                Provider = SocialAuthProvider.Google,
            };
            SocialUser? socialUser = null;
            try
            {
                socialUser = await SocialAuth.GetUser(googleAuthData);
                _testOutputHelper.WriteLine(socialUser.Name);
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
            }
            Assert.True(socialUser != null);
        }
        
        [Fact]
        public async Task TestFacebook()
        {
            var facebookAuthData = new SocialAuthData
            {
                Token = "",
                Provider = SocialAuthProvider.Facebook,
            };
            SocialUser? socialUser = null;
            try
            {
                socialUser = await SocialAuth.GetUser(facebookAuthData);
                _testOutputHelper.WriteLine(socialUser.Name??"");
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
            }
            Assert.True(socialUser != null);
        }
        
        [Fact]
        public async Task TestApple()
        {
            var appleAuthData = new SocialAuthData
            {
                Token = "",
                Provider = SocialAuthProvider.Apple,
                AppleClientId = "your_apple_client_id",
            };
            SocialUser? socialUser = null;
            try
            {
                socialUser = await SocialAuth.GetUser(appleAuthData);
                _testOutputHelper.WriteLine(socialUser.Name??"");
            }
            catch (Exception e)
            {
                _testOutputHelper.WriteLine(e.ToString());
            }
            Assert.True(socialUser != null);
        }
    }
}