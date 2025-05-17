using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NoteAPI.Repo.SqlDatabase.DTO
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("UserId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [Column("Username")]
        [StringLength(50)]
        public required string Username { get; set; }

        [Required]
        [Column("Email")]
        [StringLength(100)]
        public required string Email { get; set; }

        [Required]
        [Column("PasswordHash")]
        public required byte[] PasswordHash { get; set; }

        [Required]
        [Column("PasswordSalt")]
        public required byte[] PasswordSalt { get; set; }

        [Required]
        [Column("Role")]
        [StringLength(20)]
        public string Role { get; set; } = "User";

        [Required]
        [Column("IsActive")]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
