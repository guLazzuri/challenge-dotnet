using System.Reflection;
using challenge.Domain.Entity;
using challenge.Infrastructure.Context;
using challenge.Infrastructure.Persistence.Repositories;
using challenge.Infrastructure.Services;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
namespace Challenge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container depency injector.

            ////Criado uma vez e usamos toda vez que precisamos
            //builder.Services.AddSingleton();

            ////Tenho pre definido e termino de criar quando precisar
            //builder.Services.AddScoped();

            ////Tenho pre definido e pre criado quando precisar termino
            //builder.Services.AddTransient();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = builder.Configuration["Swagger:Title"],
                    Description = @"
                        ## 🏍️ GEF API - Sistema de Gestão de Pátio Inteligente
                        
                        Sistema digital inteligente para mapeamento e gestão de pátio de motocicletas.
                    ",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = "Gustavo Lazzuri",
                        Email = "gulazzuri@gmail.com",
                        Url = new Uri("https://github.com/guLazzuri/challenge-dotnet")
                    }
                });



                // Incluir comentários XML
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);

                // Adicionar esquemas de exemplo
                x.MapType<VehicleModel>(() => new Microsoft.OpenApi.Models.OpenApiSchema
                {
                    Type = "string",
                    Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
                    {
                        new Microsoft.OpenApi.Any.OpenApiString("E"),
                        new Microsoft.OpenApi.Any.OpenApiString("SPORT"),
                        new Microsoft.OpenApi.Any.OpenApiString("POP")
                    }
                });

                x.MapType<UserType>(() => new Microsoft.OpenApi.Models.OpenApiSchema
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

            // Repository Pattern
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
                options.ReportApiVersions = true; // retorna no header
            });


            // HATEOAS Service
            builder.Services.AddScoped<IHateoasService, HateoasService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapHealthChecks("/health");


            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
