using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IPlaceService
{
    Task<ServiceResponse<PlaceDTO>> GetPlace(Guid id);
    Task<ServiceResponse<PagedResponse<PlaceDTO>>> GetPlaces(PaginationSearchQueryParams pagination);
    Task<ServiceResponse> AddPlace(PlaceAddDTO place, UserDTO? requestingUser = null);
    Task<ServiceResponse> UpdatePlace(PlaceUpdateDTO place, UserDTO? requestingUser = null);
    Task<ServiceResponse> DeletePlace(Guid id, UserDTO? requestingUser = null);
}