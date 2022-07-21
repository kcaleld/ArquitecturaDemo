using ArquitecturaDemo.BLL.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using SistemaILP.SVL.RRHH.Infrastructure.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO.Compression;
using System.Reflection;

namespace ArquitecturaDemo.SVL.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            //LowerCaseUrls
            //Se habilita el uso de urls con minúsculas
            services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

            //Configuración de Versionamiento
            //Se registran las configuraciones para crear el versionamiento de la API con Swagger empezando con la 1
            services.ConfigureOptions<ConfigureSwaggerOptions>();
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            //CORS
            //se habilitan los CORS para permitir recibir peticiones a la API
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    //.WithOrigins("https://localhost/")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            //Validators
            //Se registran los validadores uno por uno, ya que se encuentran en otro proyecto que no es el principal
            //por lo tanto no se puede hacer un registro automático de todos los ensamblados
            services.AddValidatorsFromAssemblyContaining<UsuarioDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<RolDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UsuarioRolDtoValidator>();

            //Compression
            //Se agrega configuración para reducir el tamaño de la respuesta de la API y aumentar la velocidad de respuesta
            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>(opt => { opt.Level = CompressionLevel.Fastest; });

            return services;
        }
    }
}