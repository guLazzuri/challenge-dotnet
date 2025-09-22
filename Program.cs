using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using challenge.Controllers;
using challenge.Infrastructure.Persistence.Repositories;
using Microsoft.OpenApi.Models;
using System.Reflection;
using challenge.Infrastructure.Context;
using challenge.Domain.Entity;
using challenge.Infrastructure.Services;
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
                        
                        ### 🚀 Funcionalidades:
                        - ✅ **Paginação**: Todos os endpoints GET suportam paginação
                        - ✅ **HATEOAS**: Links de navegação automáticos
                        - ✅ **Validação**: Validação robusta de dados
                        - ✅ **Status Codes**: Códigos HTTP adequados
                        
                        ### 📋 Entidades:
                        - **Vehicles**: Gestão de motocicletas
                        - **Users**: Gestão de usuários do sistema  
                        - **MaintenanceHistories**: Histórico de manutenções
                        
                        ### 🔗 Exemplos de Uso:
                        ```
                        GET /api/Vehicles?pageNumber=1&pageSize=10
                        POST /api/Vehicles (com body JSON)
                        ```
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

            // HATEOAS Service
            builder.Services.AddScoped<IHateoasService, HateoasService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
