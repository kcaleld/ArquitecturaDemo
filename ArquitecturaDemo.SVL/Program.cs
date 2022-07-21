using ArquitecturaDemo.BLL.Config;
using ArquitecturaDemo.SVL.Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices();
builder.Services.AddBLConfig(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add logging config
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var route = "api-usuarios";
    var template = "{documentName}/docs.json";
    app.UseSwagger(options => { options.RouteTemplate = $"{route}/{template}"; });
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = route;
        foreach (var description in provider.ApiVersionDescriptions)
            options.SwaggerEndpoint($"/{route}/{description.GroupName}/docs.json", description.GroupName.ToUpperInvariant());
    });
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
