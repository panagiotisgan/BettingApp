using Betting.API.DTOModels;
using Betting.API.Services;
using Betting.API.Validators;
using Betting.DataAccess;
using Betting.DataAccess.Repositories;
using Betting.Domain;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Betting.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BettingApiDbContext>();
            services.AddScoped<IMatchRepository, MatchesRepository>();
            services.AddScoped<IMatchOddsRepository, MatchesOddsRepository>();

            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IMatchOddService, MatchOddService>();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();


            services.AddSingleton(Log.Logger);

            services.AddControllers();

            services.AddSwaggerGen();

            services.AddScoped<IValidator<MatchOddDTO>, MatchOddDTOValidator>();
            services.AddScoped<IValidator<MatchDTO>, MatchDTOValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            UpdateDatabase(app);
            if (env.IsDevelopment())
            {
                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Betting API V1.0");
                    c.RoutePrefix = String.Empty;
                });
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<BettingApiDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
