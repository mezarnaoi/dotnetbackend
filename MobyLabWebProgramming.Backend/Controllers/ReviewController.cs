using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Requests;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ReviewController : AuthorizedController
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService, IUserService userService) : base(userService)
    {
        _reviewService = reviewService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<ReviewDTO>>> GetById([FromRoute] Guid id)
    {
        return FromServiceResponse(await _reviewService.GetReview(id));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<ReviewDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination)
    {
        return FromServiceResponse(await _reviewService.GetReviews(pagination));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] ReviewAddDTO review)
    {
        var currentUser = await GetCurrentUser();
        if (currentUser.Result == null)
        {
            return ErrorMessageResult(currentUser.Error);
        }

        // Setăm UserId-ul curent în ReviewAddDTO
        review = review with { UserId = currentUser.Result.Id };

        return FromServiceResponse(await _reviewService.AddReview(review));
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] ReviewUpdateDTO review)
    {
        var currentUser = await GetCurrentUser();
        
        if (currentUser.Result == null)
        {
            return ErrorMessageResult(currentUser.Error);
        }
        
        // Verificăm dacă review-ul există
        var existingReview = await _reviewService.GetReview(review.Id);

        if (existingReview.Result == null)
        {
            return ErrorMessageResult(existingReview.Error);
        }
        
        // Verificăm dacă utilizatorul curent este același cu cel care a scris recenzia
        if (existingReview.Result.UserId != currentUser.Result.Id)
        {
            return Unauthorized("Nu aveți permisiunea să modificați această recenzie.");
        }

        return FromServiceResponse(await _reviewService.UpdateReview(review));
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();
        
        if (currentUser.Result == null)
        {
            return ErrorMessageResult(currentUser.Error);
        }
        
        // Verificăm dacă review-ul există
        var existingReview = await _reviewService.GetReview(id);

        if (existingReview.Result == null)
        {
            return ErrorMessageResult(existingReview.Error);
        }
        
        // Verificăm dacă utilizatorul curent este același cu cel care a scris recenzia
        if (existingReview.Result.UserId != currentUser.Result.Id)
        {
            return Unauthorized("Nu aveți permisiunea să modificați această recenzie.");
        }

        return FromServiceResponse(await _reviewService.DeleteReview(id));
    }
}
