using DTOs;
using PlantDiseasePOJOS;
using Services;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient<PlantAnalysisService>();

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

        // Ensure target folder
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages", upload.Foldername);
        Directory.CreateDirectory(folderPath);

        // Save uploaded file
        var filePath = Path.Combine(folderPath, upload.File.FileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await upload.File.CopyToAsync(stream);
        }

        // Prepare DTO
        var input = new InputPOJO
        {
            Foldername = upload.Foldername,
            Filename = upload.File.FileName
        };

        // Convert to base64 for Gemini
        var bytes = await File.ReadAllBytesAsync(filePath);
        var base64Image = Convert.ToBase64String(bytes);

        // 🔹 Call Gemini-powered service (async)
        var result = await service.AnalyzeAsync(input, base64Image);

        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex}");
        return Results.Problem(ex.ToString());
    }
}).DisableAntiforgery();



app.Run();

