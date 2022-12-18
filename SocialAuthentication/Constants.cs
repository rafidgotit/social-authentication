namespace Rapplis.SocialAuthentication;

public abstract class SocialAuthProvider
{
    public const string Facebook = "facebook";
    public const string Google = "google";
    public const string Apple = "apple";

    public static readonly List<string> Values = new()
    {
        Facebook,
        Google,
        Apple
    };
}