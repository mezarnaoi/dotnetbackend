using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PlaceTagController : AuthorizedController
{
    private readonly IPlaceTagService _placeTagService;

    public PlaceTagController(IUserService userService, IPlaceTagService placeTagService) : base(userService)
    {
        _placeTagService = placeTagService;
    }

    [Authorize(Roles = "Admin,Personnel")]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] PlaceTagDTO dto)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await _placeTagService.AddTagToPlace(dto))
            : ErrorMessageResult(currentUser.Error);
    }

    [Authorize(Roles = "Admin,Personnel")]
    [HttpDelete]
    public async Task<ActionResult<RequestResponse>> Remove([FromBody] PlaceTagDTO dto)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null
            ? FromServiceResponse(await _placeTagService.RemoveTagFromPlace(dto))
            : ErrorMessageResult(currentUser.Error);
    }
}