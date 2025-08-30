using PlantDiseaseApi.POJOs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<PlantAnalysisService>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors();

app.MapPost("/api/analyze", (InputPOJO input, PlantAnalysisService service) =>
{
    var result = service.Analyze(input);
    return Results.Ok(result);
});

app.Run();

internal class PlantAnalysisService
{
    internal object? Analyze(InputPOJO input)
    {
        throw new NotImplementedException();
    }
}

