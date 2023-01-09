namespace Infrastructure.Identity.Models;

public class UserRegisterDto
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    public string RefreshToken { get; set; }
}