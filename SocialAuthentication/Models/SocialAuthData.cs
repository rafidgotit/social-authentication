﻿using System.ComponentModel.DataAnnotations;

namespace Rapplis.SocialAuthentication.Models;

public class SocialAuthData
{
    [Required]
    public string Provider { get; set; }
    public string Token { get; set; }
    public string AppleClientId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}