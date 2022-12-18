namespace Rapplis.SocialAuthentication.Models;

public class SocialUser
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    public bool? EmailVerified { get; set; }
    public string? Name { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Picture { get; set; }
    public string? Locale { get; set; }
    public string? Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string Provider { get; set; }
}