using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MoviesAPI.Filters;
using MoviesAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options=>
            {
                // pridavam globalny filter pre chyby
                options.Filters.Add(typeof(MyExceptionFilter));
            });

            // druhy filtrov: Authorization, Resource, Action, Exceprion, Result....v ramci akci, kontroleru, alebo globalne...
            // vacsinou (alebo vzdy) ako atributy
            // toto je asi globalny filter
            services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MoviesAPI", Version = "v1" });
            });

            services.AddSingleton<IRepository, InMemoryRepository>();
            services.AddTransient<MyActionFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Konfiguruje sa tu tzv. Request Pipeline, cize sled procesov pri prijati poziadavky, pricom nastavenie poradia je dolezite!!!
        // IHostEnviroment umoznuje urcit, v akom sme prostredi, ci Development alebo Production
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // Use znamena, ze middleware sa vykona a neukonci reequest ale posunie vysledok v pipe dalej na dalsi middleware
            // Run() neposunie vysledok, ale vykonanim sa request ukonci!!!
            // toto je vraj nejake loggovanie kazdeho requestu/alebo responsu?....ale inac tomu vobec nerozumien :((
            // tusim logovanie kazdeho response body...aspon tak to povedal Gavilanko :)
            /*app.Use(async (context, next) =>
            {
                using(var swapStream=new MemoryStream())
                {
                    var originalResponseBody = context.Response.Body;
                    context.Response.Body = swapStream;

                    await next.Invoke();  // odovzda request na dalsi middleware

                    swapStream.Seek(0, SeekOrigin.Begin);
                    string responseBody = new StreamReader(swapStream).ReadToEnd();
                    swapStream.Seek(0, SeekOrigin.Begin);

                    await swapStream.CopyToAsync(originalResponseBody);
                    context.Response.Body = originalResponseBody;

                    logger.LogInformation(responseBody);
                }
            });*/

            // priklad vlastneho middlewaru, ktorym sa napr. ukonci pipe-line
            // bez ohladu na endpoint, vzdy sa vrati tento oznam
            //app.Run(async context =>
            //{
            //    context.Response.ContentType = "text/html; charset=UTF-8";
            //    await context.Response.WriteAsync("Tady konèíš, zahradníku!!");
            //});

            // toto je praktickejsie, mapujem moj middleware na konkretny endpoint
            app.Map("/map1", (app) =>
            {
                app.Run(async context =>
                {
                    context.Response.ContentType = "text/html; charset=UTF-8";
                    await context.Response.WriteAsync("Tady konèíš, zahradníku!!");
                });
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MoviesAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // caching filter
            app.UseResponseCaching();

            // pridanie autentifikacie cey JwToken
            app.UseAuthentication();

            // napr. Autorizacia musi byt pred sparcovanim poziadavky cez MVC
            app.UseAuthorization();

            // vlastne MVC-middleware
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
