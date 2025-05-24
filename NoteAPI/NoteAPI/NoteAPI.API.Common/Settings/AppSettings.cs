using System.ComponentModel.DataAnnotations;

namespace NoteAPI.API.Common.Settings
{
    public class AppSettings
    {
        [Required]
        public ApiSettings API { get; set; }
        [Required]
        public Swagger Swagger { get; set; }
    }

    public class ApiSettings
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public ApiContact Contact { get; set; }

        public string TermsOfServiceUrl { get; set; }

        public ApiLicense License { get; set; }

        public JwtSettings JwtSettings { get; set; }  
        public Admin Admin { get; set; }
    }

    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
    public class Admin
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ApiContact
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string Url { get; set; }
    }

    public class ApiLicense
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Swagger
    {
        [Required]
        public bool Enabled { get; set; }
    }

    public class ExternalServices
    {
        public ForecastService ForecastService { get; set; }
    }

    public class ForecastService
    {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string Key { get; set; }
    }
}
