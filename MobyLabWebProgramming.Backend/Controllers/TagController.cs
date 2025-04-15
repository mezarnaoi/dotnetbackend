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
public class TagController : AuthorizedController
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService, IUserService userService) : base(userService)
    {
        _tagService = tagService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<TagDTO>>> GetById([FromRoute] Guid id)
    {
        return FromServiceResponse(await _tagService.GetTag(id));
    }

    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<TagDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination)
    {
        return FromServiceResponse(await _tagService.GetTags(pagination));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] TagAddDTO dto)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await _tagService.AddTag(dto)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] TagUpdateDTO dto)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await _tagService.UpdateTag(dto)) :
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ?
            FromServiceResponse(await _tagService.DeleteTag(id)) :
            ErrorMessageResult(currentUser.Error);
    }
}
