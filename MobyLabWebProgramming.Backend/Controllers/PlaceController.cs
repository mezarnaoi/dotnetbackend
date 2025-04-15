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
public class PlaceController(IPlaceService placeService, IUserService userService) : AuthorizedController(userService)
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<PlaceDTO>>> GetById([FromRoute] Guid id)
    {
        return FromServiceResponse(await placeService.GetPlace(id));
    }

    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<PlaceDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination)
    {
        return FromServiceResponse(await placeService.GetPlaces(pagination));
    }

    [Authorize(Roles = "Admin,Personnel")]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] PlaceAddDTO place)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await placeService.AddPlace(place, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize(Roles = "Admin,Personnel")]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] PlaceUpdateDTO place)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await placeService.UpdatePlace(place, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize(Roles = "Admin,Personnel")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await placeService.DeletePlace(id, currentUser.Result)) :
            ErrorMessageResult(currentUser.Error);
    }
}
