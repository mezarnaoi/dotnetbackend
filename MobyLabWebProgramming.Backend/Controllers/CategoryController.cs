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
public class CategoryController : AuthorizedController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService, IUserService userService)
        : base(userService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RequestResponse<CategoryDTO>>> GetById([FromRoute] Guid id)
    {
        return FromServiceResponse(await _categoryService.GetCategory(id));
    }

    [HttpGet]
    public async Task<ActionResult<RequestResponse<PagedResponse<CategoryDTO>>>> GetPage([FromQuery] PaginationSearchQueryParams pagination)
    { 
        return FromServiceResponse(await _categoryService.GetCategories(pagination));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Add([FromBody] CategoryAddDTO category)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ? 
            FromServiceResponse(await _categoryService.AddCategory(category)) : 
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> Update([FromBody] CategoryUpdateDTO category)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ? 
            FromServiceResponse(await _categoryService.UpdateCategory(category)) : 
            ErrorMessageResult(currentUser.Error);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<RequestResponse>> Delete([FromRoute] Guid id)
    {
        var currentUser = await GetCurrentUser();
        return currentUser.Result != null ? 
            FromServiceResponse(await _categoryService.DeleteCategory(id)) : 
            ErrorMessageResult(currentUser.Error);
    }
}
