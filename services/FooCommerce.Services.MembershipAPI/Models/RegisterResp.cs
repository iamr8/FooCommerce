﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FooCommerce.Services.MembershipAPI.Models;

[Serializable]
public record RegisterReq
{
    /// <summary>
    /// First Name of the User.
    /// </summary>
    [Required, JsonRequired, JsonPropertyName("firstName"), RegularExpression(@"^[a-zA-Z ]{2,20}$")]
    public string FirstName { get; init; }
    /// <summary>
    /// Last Name of the User.
    /// </summary>
    [Required, JsonRequired, JsonPropertyName("lastName"), RegularExpression(@"^[a-zA-Z ]{2,20}$")]
    public string LastName { get; init; }
    /// <summary>
    /// Email of the User.
    /// </summary>
    [Required, JsonRequired, JsonPropertyName("email")]
    [EmailAddress]
    public string Email { get; init; }
    /// <summary>
    /// Password of the User.
    /// </summary>
    [Required, JsonRequired, JsonPropertyName("password"), MinLength(8)]
    public string Password { get; init; }
}

public interface IRegisterResp { }

[Serializable]
public record RegisterResp : IRegisterResp
{
    /// <summary>
    /// User's Communication Id.
    /// </summary>
    [JsonPropertyName("commId")]
    public Guid? CommunicationId { get; init; }
}
[Serializable]
public record RegisterRespEmpty : IRegisterResp
{
}
[Serializable]
public record RegisterRespFaulted : IRegisterResp
{
    /// <summary>
    /// Error Message.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; init; }
}