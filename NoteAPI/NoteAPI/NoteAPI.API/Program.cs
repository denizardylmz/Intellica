using System.IO;
using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

#pragma warning disable CS1591
namespace NoteAPI.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //.ConfigureKestrel(options =>
            //{
            //    options.ListenAnyIP(5001); //Evdeki cihazları dinlemesi için.
            //})
            .UseStartup<Startup>();
    }
}
