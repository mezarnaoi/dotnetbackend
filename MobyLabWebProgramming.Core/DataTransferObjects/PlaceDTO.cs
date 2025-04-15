namespace MobyLabWebProgramming.Core.DataTransferObjects;

public record PlaceDTO(Guid Id, string Name, string Address, string Description, Guid CategoryId, List<TagMiniDTO> Tags, List<ReviewDTO> Reviews);