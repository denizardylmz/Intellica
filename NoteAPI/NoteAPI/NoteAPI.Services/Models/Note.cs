using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteAPI.Services.Models
{
    public class Note
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsArchived { get; set; } = false;
        public string? CreatedBy { get; set; }
        public string? Tags { get; set; }
    }
}
