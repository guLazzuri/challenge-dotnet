using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = "Challenge API",
                Version = description.ApiVersion.ToString(),
                Description = "Sistema de Gestão de Pátio Inteligente",
                Contact = new OpenApiContact
                {
                    Name = "Gustavo Lazzuri",
                    Email = "gulazzuri@gmail.com",
                    Url = new Uri("https://github.com/guLazzuri/challenge-dotnet")
                }
            });
        }
    }
}