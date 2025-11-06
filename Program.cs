using System.Reflection;
using System.Text;
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

namespace Challenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ---------------------------
            // DEPENDENCY INJECTION
            // ---------------------------
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // ---------------------------
            // SWAGGER CONFIGURATION
            // ---------------------------
            builder.Services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API TRACKIN - MOTTU",
                    Description = "Swagger da API TRACKIN - MOTTU.",
                    Contact = new OpenApiContact
                    {
                        Name = "Leandro Correia",
                        Email = "rm556203@fiap.com.br"
                    }
                });

                // Configuração JWT no Swagger
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Digite **'Bearer {seu_token}'** para autenticar."
                };

                x.AddSecurityDefinition("Bearer", securityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
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
                };

                x.AddSecurityRequirement(securityRequirement);

                // XML comments (para documentação)
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    x.IncludeXmlComments(xmlPath);
            });

            // ---------------------------
            // DATABASE CONFIG (Oracle)
            // ---------------------------
            builder.Services.AddDbContext<ChallengeContext>(options =>
            {
                options.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
            });

            // ---------------------------
            // REPOSITORIES & SERVICES
            // ---------------------------
            builder.Services.AddScoped<IRepository<Vehicle>, Repository<Vehicle>>();
            builder.Services.AddScoped<IRepository<User>, Repository<User>>();
            builder.Services.AddScoped<IRepository<MaintenanceHistory>, Repository<MaintenanceHistory>>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<IHateoasService, HateoasService>();

            // ---------------------------
            // HEALTH CHECK
            // ---------------------------
            builder.Services.AddHealthChecks()
                .AddOracle(
                    connectionString: builder.Configuration.GetConnectionString("Oracle"),
                    name: "oracle",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] { "db", "oracle" }
                );

            // ---------------------------
            // API VERSIONING
            // ---------------------------
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            // ---------------------------
            // JWT AUTHENTICATION
            // ---------------------------
            var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key não configurada no appsettings.json"));

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"]
                };
            });

            // ---------------------------
            // BUILD APPLICATION
            // ---------------------------
            var app = builder.Build();

            // ---------------------------
            // MIDDLEWARE PIPELINE
            // ---------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
