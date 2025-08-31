using DTOs;
using PlantDiseasePOJOS;
using Services;
using Microsoft.AspNetCore.Mvc;


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

app.MapPost("/api/analyze", async ([FromForm] FileUploadDTO upload, PlantAnalysisService service) =>
{
    try
    {
        Console.WriteLine($"Received file: {upload.File?.FileName}, Folder: {upload.Foldername}");

        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages", upload.Foldername);
        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, upload.File.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await upload.File.CopyToAsync(stream);
        }

        var input = new InputPOJO
        {
            Foldername = upload.Foldername,
            Filename = upload.File.FileName
        };

        var result = service.Analyze(input, filePath);
        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        return Results.Problem(ex.ToString());
    }
}).DisableAntiforgery();


app.Run();

