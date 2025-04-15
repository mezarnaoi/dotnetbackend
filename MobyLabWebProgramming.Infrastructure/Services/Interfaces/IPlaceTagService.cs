using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IPlaceTagService
{
    Task<ServiceResponse> AddTagToPlace(PlaceTagDTO dto);
    Task<ServiceResponse> RemoveTagFromPlace(PlaceTagDTO dto);
    
}