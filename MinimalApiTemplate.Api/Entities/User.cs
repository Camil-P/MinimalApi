using MinimalApiTemplate.Api.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalApiTemplate.Api.Entities;

public class User : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Country { get; set; }
    public Role Role { get; set; } = Role.BasicUser;

    public string ProfileImageUrl { get; set; } = "https://cdn-icons-png.flaticon.com/512/3177/3177440.png";

    public DateTime? EmailConfirmation { get; set; }
    [NotMapped]
    public bool EmailConfirmed => EmailConfirmation.HasValue;

    public DateTime? Banned { get; set; }
    [NotMapped]
    public bool IsBanned => Banned.HasValue;

    public DateTime? PasswordReset { get; set; }
}
