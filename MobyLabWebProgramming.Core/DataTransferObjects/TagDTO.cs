namespace MobyLabWebProgramming.Core.DataTransferObjects;

public record TagDTO(
    Guid Id,
    string Name,
    List<PlaceMiniDTO> Places // locuri asociate cu acest tag
);