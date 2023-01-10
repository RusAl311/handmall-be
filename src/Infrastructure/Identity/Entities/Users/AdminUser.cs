using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Identity.Entities.Users;

public class AdminUser
{
    [Key]
    public int Id { get; set; }

    public string Username { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public byte[] PasswordHash { get; init; }
    public byte[] PasswordSalt { get; init; }
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }
    #nullable enable
    public RefreshToken? RefreshToken { get; set; }
}