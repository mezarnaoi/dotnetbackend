using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface ITagService
{
    Task<ServiceResponse<TagDTO>> GetTag(Guid id);
    Task<ServiceResponse<PagedResponse<TagDTO>>> GetTags(PaginationSearchQueryParams pagination);
    Task<ServiceResponse> AddTag(TagAddDTO dto);
    Task<ServiceResponse> UpdateTag(TagUpdateDTO dto);
    Task<ServiceResponse> DeleteTag(Guid id);
}