namespace MobyLabWebProgramming.Core.DataTransferObjects;

public record ReviewMiniDTO(
    Guid Id,
    string Content,
    int Rating,
    Guid PlaceId
);