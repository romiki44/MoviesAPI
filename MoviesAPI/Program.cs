using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // len priklad bez DI
            // sice triedy su volne previazane, ale zavislosti vytvaraju neprijemnu retaz pri volani konstruktora!!
            //var genresController=new GenresController(new InMemoryRepository(new Logger()))

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // moznost nakonfigurovat providera pre Logging service - to jest databaza, konzola a pod..!
                    //webBuilder.ConfigureLogging()
                    webBuilder.UseStartup<Startup>();
                });
    }
}
