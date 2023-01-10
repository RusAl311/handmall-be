using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Identity.Entities.Users;

public class RefreshToken
{
    [Key]
    public int Id { get; set; }
    #nullable enable
    public string? Token { get; set; }
    public DateTime Created = DateTime.Now;
    public DateTime Expires { get; set; }
    [ForeignKey("AdminUser")]
    public int AdminUserId { get; set; }
    public AdminUser? AdminUser { get; set; }
}