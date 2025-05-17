using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteAPI.Repo.SqlDatabase.Core
{
    public abstract class IAuditable
    {

        [MaxLength(100)]
        public string? CreatedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [MaxLength(100)]
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
