using challenge.Domain.Entity;
using challenge.Infrastructure.Context;
using challenge.Infrastructure.Persistence.Repositories;
using challenge.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Challenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jwtKey = builder.Configuration["Jwt:Key"];
            var jwtIssuer = builder.Configuration["Jwt:Issuer"];
            var jwtAudience = builder.Configuration["Jwt:Audience"];

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = builder.Configuration["Swagger:Title"],
                    Description = builder.Configuration["Swagger:Description"],
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = builder.Configuration["Swagger:Name"],
                        Email = builder.Configuration["Swagger:Email"],
                        Url = new Uri("https://github.com/guLazzuri/challenge-dotnet")
                    }
                });

                // ✅ Ativa o botão "Authorize"
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT no formato: Bearer {seu token}"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);

                x.MapType<VehicleModel>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
                    {
                        new Microsoft.OpenApi.Any.OpenApiString("E"),
                        new Microsoft.OpenApi.Any.OpenApiString("SPORT"),
                        new Microsoft.OpenApi.Any.OpenApiString("POP")
                    }
                });

                x.MapType<UserType>(() => new OpenApiSchema
                {
                    Type = "string",
                    Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
                    {
                        new Microsoft.OpenApi.Any.OpenApiString("ADMIN"),
                        new Microsoft.OpenApi.Any.OpenApiString("CLIENT")
                    }
                });
            });

            builder.Services.AddDbContext<ChallengeContext>(options =>
            {
                options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
            });

            builder.Services.AddScoped<IRepository<Vehicle>, Repository<Vehicle>>();
            builder.Services.AddScoped<IRepository<User>, Repository<User>>();
            builder.Services.AddScoped<IRepository<MaintenanceHistory>, Repository<MaintenanceHistory>>();

            builder.Services.AddHealthChecks()
                .AddOracle(
                    connectionString: builder.Configuration.GetConnectionString("Oracle"),
                    name: "oracle",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "db", "oracle" }
                );

            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            builder.Services.AddScoped<IHateoasService, HateoasService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapHealthChecks("/health");
            app.UseExceptionHandler("/error");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}