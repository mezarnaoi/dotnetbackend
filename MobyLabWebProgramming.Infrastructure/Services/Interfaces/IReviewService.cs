using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;

namespace MobyLabWebProgramming.Infrastructure.Services.Interfaces;

public interface IReviewService
{
    Task<ServiceResponse<ReviewDTO>> GetReview(Guid id);
    Task<ServiceResponse<PagedResponse<ReviewDTO>>> GetReviews(PaginationSearchQueryParams pagination);
    Task<ServiceResponse> AddReview(ReviewAddDTO dto);
    Task<ServiceResponse> UpdateReview(ReviewUpdateDTO dto);
    Task<ServiceResponse> DeleteReview(Guid id);
}