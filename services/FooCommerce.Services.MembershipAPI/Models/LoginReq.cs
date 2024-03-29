﻿using System.Text.Json.Serialization;

namespace FooCommerce.MembershipService.Models;

[Serializable]
public record LoginReq
{
    [JsonRequired, JsonPropertyName("username")]
    public string Username { get; init; }
    [JsonRequired, JsonPropertyName("password")]
    public string Password { get; init; }
}

[Serializable]
public record LoginResp
{
}