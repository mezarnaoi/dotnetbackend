using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface ICategoryService
{
    Task<ServiceResponse<CategoryDTO>> GetCategory(Guid id);
    Task<ServiceResponse<PagedResponse<CategoryDTO>>> GetCategories(PaginationSearchQueryParams pagination);
    Task<ServiceResponse> AddCategory(CategoryAddDTO dto);
    Task<ServiceResponse> UpdateCategory(CategoryUpdateDTO dto);
    Task<ServiceResponse> DeleteCategory(Guid id);
}