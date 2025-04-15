namespace MobyLabWebProgramming.Core.DataTransferObjects;

public record ReviewUpdateDTO(
    Guid Id,
    string Content,
    int Rating
);