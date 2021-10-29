using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SportStore.Data;
using SportStore.Filters;
using SportStore.Helpers;
using SportStore.Managers;
using SportStore.Managers.Repositories;
using SportStore.Models.Entities;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SportStore.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>
                (option =>
                {
                    option.UseSqlServer(configuration.GetConnectionString("DeafultConnetionString"));
                    option.EnableSensitiveDataLogging();
                });

            services.AddAutoMapper(typeof(Program));

            services.AddTransient<IUserManager, UserManager>();
            services.AddTransient<IAuthenticationManager, AuthenticationManager>();

            services.AddScoped<IStoreRepository<Product>, ProductRepository>();
            //// TODO: Implementing CategoryRepository & OrderRepository
            //services.AddScoped<IStoreRepository<Category>, CategoryRepository>();
            //services.AddScoped<IStoreRepository<Order>, OrderRepository>();
        }

        public static void ConfigureMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add<JsonExceptionFilter>();

                //var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddControllers()
                    .AddNewtonsoftJson();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearer"
                            }
                        },
                        new List<string>()
                    }
                });

                c.OperationFilter<SwaggerDefaultValues>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

                //c.OrderActionsBy(k => $"{k.RelativePath}_{k.HttpMethod}");
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

                c.EnableAnnotations();
            })
                .AddSwaggerGenNewtonsoftSupport();
        }

        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                // Comment The next line and add "?api-version=2.0" in Query String
                options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version"); // Header Versioning 
            });
        }

        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        //ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                        //ValidAudience = jwtSettings.GetSection("validAudience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("secret").Value))
                    };
                });
        }

        public static void ConfigureSecurity(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://localhost:44348")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            services.AddAuthorization(opt =>
            {
                // If the User will satisfy The Policy In case he has a claim of "ClaimKey"
                opt.AddPolicy("HasTestClaimPolicy", policy => policy.RequireClaim("ClaimKey"));
                opt.AddPolicy("HasTestKVClaimPolicy", policy => policy.RequireClaim("Key", "Value"));
                // Role
                opt.AddPolicy("IsUserPolicy", policy => policy.RequireRole("User"));
                // Compaine => NT: The user should siticfy them all.
                opt.AddPolicy("SuperUserPolicy", p =>
                {
                    p.RequireRole("Admin");
                    p.RequireAssertion(c => c.User.IsInRole("User"));
                    p.RequireClaim("Owner", "Yes");
                });
            });
        }

    }
}

