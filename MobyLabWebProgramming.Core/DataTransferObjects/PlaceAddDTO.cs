namespace MobyLabWebProgramming.Core.DataTransferObjects;

public record PlaceAddDTO(
    string Name,
    string Address,
    string Description,
    Guid CategoryId,
    List<Guid> TagIds
);