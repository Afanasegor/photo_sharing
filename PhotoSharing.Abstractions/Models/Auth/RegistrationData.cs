using System.ComponentModel.DataAnnotations;

namespace PhotoSharing.Abstractions.Models.Auth
{
    public class RegistrationData
    {
        [MinLength(4, ErrorMessage = "Name cannot be less than 4.")]
        [MaxLength(36, ErrorMessage = "Name cannot be greater than 36.")]
        [Required] public string Name { get; set; } = null!;

        [Required, EmailAddress] public string Email { get; set; } = null!;

        [Required] public string Password { get; set; } = null!;

        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        [Required] public string ConfirmPassword { get; set; } = null!;
    }
}
