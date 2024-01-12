using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Notes.Domain;

public class User : IdentityUser
{
    [PersonalData]
    [Required]
    public string? Name { get; set; }

    [PersonalData]
    [Required]
    public DateOnly BirthDate { get; set; }

    [EmailAddress]
    [Required]
    public override string? Email { get; set; }

    [Required]
    [MinLength(5)]
    [RegularExpression("^[a-zA-Z0-9]*$")]
    public override string? UserName { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? InactivationDate { get; set; } = null;
}
