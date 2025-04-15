namespace MobyLabWebProgramming.Core.DataTransferObjects;

public record ReviewDTO(
    Guid Id,
    string Content,
    int Rating,
    Guid UserId,
    Guid PlaceId,
    DateTime CreatedAt
);