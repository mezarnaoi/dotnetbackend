namespace MobyLabWebProgramming.Core.DataTransferObjects;

public record CategoryDTO(
    Guid Id,
    string Name,
    List<PlaceMiniDTO> Places
);