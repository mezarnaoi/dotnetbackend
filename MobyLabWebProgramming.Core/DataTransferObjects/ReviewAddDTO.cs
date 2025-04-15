using System.Text.Json.Serialization;

namespace MobyLabWebProgramming.Core.DataTransferObjects;

public record ReviewAddDTO(
    string Content,
    int Rating,
    [property: JsonIgnore] Guid UserId, // Vom ignora UserId în Swagger
    Guid PlaceId
);