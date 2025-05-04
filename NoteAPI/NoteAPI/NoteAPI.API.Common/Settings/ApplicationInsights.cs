using System.ComponentModel.DataAnnotations;

namespace NoteAPI.API.Common.Settings
{
    public class ApplicationInsights
    {
        [Required]
        public bool Enabled { get; set; }

        public string ConnectionString { get; set; }
    }
}
