namespace SportStore.Helpers;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        this._provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var info = new OpenApiInfo
            {
                Title = $"{Assembly.GetExecutingAssembly().GetName().Name} {description.ApiVersion}",
                Description = "Simple Application To Test Web API Swagger Documentaion !!..",
                Version = description.ApiVersion.ToString(),
                TermsOfService = new Uri("https://google.com/"),
                Contact = new OpenApiContact
                {
                    Name = "Ahmed HENNANI",
                    Email = "hennani.220@gmail.com",
                    Url = new Uri("https://twitter.com/ah-hnaniii"),
                },
                License = new OpenApiLicense
                {
                    Name = "Use under License",
                    Url = new Uri("https://linkedin.com/"),
                }
            };

            options.SwaggerDoc(description.GroupName, info);
        }
    }
}
