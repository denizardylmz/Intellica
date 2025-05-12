using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteAPI.Repo.SqlDatabase.DTO
{
    [Table("Notes")]
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string? Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public bool IsArchived { get; set; } = false;

        [MaxLength(100)]
        public string? CreatedBy { get; set; }

        [MaxLength(300)]
        public string? Tags { get; set; }
    }
}
