using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FooCommerce.Services.ProductAPI.Models;

[Serializable]
public record CreateCategoryReq
{
    [JsonPropertyName("groupId")]
    public int GroupId { get; set; }

    [Required, JsonRequired, JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("icon")]
    public string Icon { get; set; }
    [JsonPropertyName("desc")]
    public string Description { get; set; }
    [JsonPropertyName("parentId")]
    public int? ParentId { get; set; }
}

public interface ICreateCategoryResp { }

[Serializable]
public record CreateCategoryResp : ICreateCategoryResp
{
}

[Serializable]
public record CreateCategoryRespFaulted : ICreateCategoryResp
{
    public CreateCategoryFaults Status { get; set; }
}

public enum CreateCategoryFaults
{
    ParentNotFound = 0,
    GroupNotFound = 1,
    AlreadyExists = 2
}