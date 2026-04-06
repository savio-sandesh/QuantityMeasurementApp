using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ModelLayer
{
    [Index(nameof(Email), IsUnique = true)]
    [Table("Users")]
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(120)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(160)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? PasswordHash { get; set; }

        [Required]
        [MaxLength(30)]
        public string Role { get; set; } = "User";

        [Required]
        [MaxLength(30)]
        public string AuthProvider { get; set; } = "Local";

        [MaxLength(200)]
        public string? ProviderUserId { get; set; }

        public bool EmailVerified { get; set; } = false;
    }
}