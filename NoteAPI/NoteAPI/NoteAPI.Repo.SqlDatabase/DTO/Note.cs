using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoteAPI.Repo.SqlDatabase.Core;

namespace NoteAPI.Repo.SqlDatabase.DTO
{
    [Table("Notes")]
    public class Note : IAuditable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public string? Content { get; set; }
        [Required]
        public bool IsArchived { get; set; } = false;
        [MaxLength(300)]
        public string? Tags { get; set; }
    }
}
