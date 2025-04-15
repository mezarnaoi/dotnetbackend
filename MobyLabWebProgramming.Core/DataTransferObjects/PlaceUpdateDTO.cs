namespace MobyLabWebProgramming.Core.DataTransferObjects;

public record PlaceUpdateDTO(
    Guid Id,
    string Name,
    string Address,
    string Description,
    Guid CategoryId,
    List<Guid> TagIds
);