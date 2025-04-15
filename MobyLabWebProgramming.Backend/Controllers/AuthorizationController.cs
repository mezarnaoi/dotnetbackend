using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobyLabWebProgramming.Core.DataTransferObjects;
using MobyLabWebProgramming.Core.Enums;
using MobyLabWebProgramming.Core.Handlers;
using MobyLabWebProgramming.Core.Responses;
using MobyLabWebProgramming.Infrastructure.Authorization;
using MobyLabWebProgramming.Infrastructure.Services.Implementations;
using MobyLabWebProgramming.Infrastructure.Services.Interfaces;

namespace MobyLabWebProgramming.Backend.Controllers;

/// <summary>
/// This is a controller to respond to authentication requests.
/// Inject the required services through the constructor.
/// </summary>
[ApiController] // This attribute specifies for the framework to add functionality to the controller such as binding multipart/form-data.
[Route("api/[controller]/[action]")] // The Route attribute prefixes the routes/url paths with template provides as a string, the keywords between [] are used to automatically take the controller and method name.
public class AuthorizationController(IUserService userService) : BaseResponseController // The controller must inherit ControllerBase or its derivations, in this case BaseResponseController.
{
    /// <summary>
    /// This method will respond to login requests.
    /// </summary>
    [HttpPost] // This attribute will make the controller respond to a HTTP POST request on the route /api/Authorization/Login having a JSON body deserialized as a LoginDTO.
    public async Task<ActionResult<RequestResponse<LoginResponseDTO>>> Login([FromBody] LoginDTO login) // The FromBody attribute indicates that the parameter is deserialized from the JSON body.
    {
        return FromServiceResponse(await userService.Login(login with { Password = PasswordUtils.HashPassword(login.Password)})); // The "with" keyword works only with records and it creates another object instance with the updated properties. 
    }
    
    [HttpPost]
    public async Task<ActionResult<RequestResponse>> Register([FromBody] RegisterDTO register)
    {
        var addUserResult = await userService.AddUser(new UserAddDTO
        {
            Email = register.Email,
            Name = register.Name,
            Password = PasswordUtils.HashPassword(register.Password),
            Role = UserRoleEnum.Client // setezi automat rolul
        });

        return FromServiceResponse(addUserResult);
    }
    
    private UserClaims? _userClaims;
    private readonly IUserService UserService = userService;
    
    private UserClaims ExtractClaims()
    {
        if (_userClaims != null)
        {
            return _userClaims;
        }

        var enumerable = User.Claims.ToList();
        var userId = enumerable.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => Guid.Parse(x.Value)).FirstOrDefault();
        var email = enumerable.Where(x => x.Type == ClaimTypes.Email).Select(x => x.Value).FirstOrDefault();
        var name = enumerable.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();

        _userClaims = new(userId, name, email);

        return _userClaims;
    }
    
    [Authorize]
    [HttpDelete]
    public async Task<ActionResult<RequestResponse>> DeleteCurrentUser()
    {
        var currentUser = await UserService.GetUser(ExtractClaims().Id);
        if (currentUser.Result == null)
            return ErrorMessageResult(currentUser.Error);

        return FromServiceResponse(await userService.DeleteUser(currentUser.Result.Id));
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<RequestResponse>> UpdateCurrentUser([FromBody] UserSelfUpdateDTO user)
    {
        var currentUser = await UserService.GetUser(ExtractClaims().Id);
        if (currentUser.Result == null)
            return ErrorMessageResult(currentUser.Error);

        var updatedUser = new UserUpdateDTO(
            Id: currentUser.Result.Id,
            Name: user.Name,
            Password: !string.IsNullOrWhiteSpace(user.Password) ? PasswordUtils.HashPassword(user.Password) : null
        );

        return FromServiceResponse(await userService.UpdateUser(updatedUser, currentUser.Result));
    }

}
