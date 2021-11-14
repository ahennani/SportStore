using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportStore.Data;
using SportStore.Extensions;
using SportStore.Models;
using System;

namespace SportStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureMvc();
            services.ConfigureSwagger();
            services.ConfigureSecurity();
            services.ConfigureApiVersioning();
            services.ConfigureJwt(Configuration);
            services.ConfigureServices(Configuration);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                // Fires when The ModelState is not valid.
                options.InvalidModelStateResponseFactory = context =>
                {
                        var errorResponse = new ApiError(context.ModelState);
                        return new BadRequestObjectResult(errorResponse);
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // Customize the UI
                //options.InjectStylesheet("/swagger-ui/custom.css");

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                options.RoutePrefix = String.Empty;
            });
        }
    }
}
