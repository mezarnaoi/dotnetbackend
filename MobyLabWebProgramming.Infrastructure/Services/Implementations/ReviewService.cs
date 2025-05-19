using System.Net;
using Microsoft.EntityFrameworkCore;
using MobyLabWebProgramming.Core.Constants;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Entities;
using MobyLabWebProgramming.Core.Errors;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Database;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Infrastructure.Services;

public class ReviewService : IReviewService
{
    private readonly WebAppDatabaseContext _dbContext;
    private readonly IMailService _mailService;
    
    public ReviewService(WebAppDatabaseContext dbContext, IMailService mailService)
    {
        _dbContext = dbContext;
        _mailService = mailService;
    }

    public async Task<ServiceResponse<ReviewDTO>> GetReview(Guid id)
    {
        var review = await _dbContext.Reviews
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new ReviewDTO(r.Id, r.Content, r.Rating, r.UserId, r.PlaceId, r.CreatedAt))
            .FirstOrDefaultAsync();

        if (review is not null)
        {
            return ServiceResponse<ReviewDTO>.ForSuccess(review);
        }

        return ServiceResponse<ReviewDTO>.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Recenzia nu a fost găsită.")) as ServiceResponse<ReviewDTO>;
    }

    public async Task<ServiceResponse<PagedResponse<ReviewDTO>>> GetReviews(PaginationSearchQueryParams pagination)
    {
        var query = _dbContext.Reviews.AsNoTracking();

        var total = await query.CountAsync();

        var result = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip(pagination.Page * pagination.ElementsPerPage)
            .Take(pagination.ElementsPerPage)
            .Select(r => new ReviewDTO(r.Id, r.Content, r.Rating, r.UserId, r.PlaceId, r.CreatedAt))
            .ToListAsync();

        return ServiceResponse<PagedResponse<ReviewDTO>>.ForSuccess(
            new PagedResponse<ReviewDTO>(pagination.Page, pagination.ElementsPerPage, total, result));
    }

    public async Task<ServiceResponse> AddReview(ReviewAddDTO dto)
    {
        var review = new Review
        {
            Id = Guid.NewGuid(),
            Content = dto.Content,
            Rating = dto.Rating,
            PlaceId = dto.PlaceId,
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Reviews.Add(review);
        await _dbContext.SaveChangesAsync();
        
        // ⬇️ Fetch user & place info
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == dto.UserId);
        var place = await _dbContext.Places.AsNoTracking().FirstOrDefaultAsync(p => p.Id == dto.PlaceId);

        if (user is not null && place is not null)
        {
            var body = MailTemplates.ReviewAddTemplate(user.Name, place.Name, dto.Rating, dto.Content);
            await _mailService.SendMail(user.Email, "Mulțumim pentru recenzie!", body, true, "ReviewVerse");
        }

        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> UpdateReview(ReviewUpdateDTO dto)
    {
        var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == dto.Id);

        if (review is null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Recenzia nu a fost găsită."));
        }

        review.Content = dto.Content;
        review.Rating = dto.Rating;
        review.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return ServiceResponse.ForSuccess();
    }

    public async Task<ServiceResponse> DeleteReview(Guid id)
    {
        var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);

        if (review is null)
        {
            return ServiceResponse.FromError(new ErrorMessage(HttpStatusCode.NotFound, "Recenzia nu a fost găsită."));
        }

        _dbContext.Reviews.Remove(review);
        await _dbContext.SaveChangesAsync();

        return ServiceResponse.ForSuccess();
    }
}
