namespace SportStore;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.ConfigureMvc();
        services.ConfigureMapper();
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
