namespace MobyLabWebProgramming.Core.DataTransferObjects;

public class RegisterDTO
{
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Password { get; set; } = default!;
}