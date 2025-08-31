using System.Net.Http;
using System.Text;
using System.Text.Json;
using PlantDiseasePOJOS;
using DTOs;

namespace Services
{
    public class PlantAnalysisService
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        public PlantAnalysisService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<OutputPOJO?> AnalyzeAsync(InputPOJO input, string imageBase64)
        {
            var prompt = @"
Analyze the provided image of a plant leaf for any signs of disease. Generate a JSON object containing the analysis results. The entire output must be valid JSON only, with no additional text or explanations before or after the code block. Populate the JSON fields based on your analysis of the image, adhering strictly to the provided format. If the image contains multiple distinct diseases or different leaves, add a new object to the plant_analysis array for each one. JSON Format: { ""plant_analysis"": [ { ""plant_name"": ""string"", ""detection_confidence_percent"": ""number"", ""status"": ""string (e.g., Normal, Abnormal)"", ""primary_observation"": ""string (plain English description of abnormalities)"", ""disease_name"": ""string"", ""disease_confidence_percent"": ""number"", ""scientific_plant_name"": ""string (e.g., Solanum tuberosum)"", ""scientific_disease_name"": ""string (e.g., Phytophthora infestans)"", ""type"": ""string (e.g., Fungus, Bacteria, Virus, Insect, etc.)"", ""severity"": ""string (e.g., Grade 1, 2, 3, 4 or Light, Moderate, Severe)"", ""spread_risk"": ""string (e.g., Low, Medium, High)"", ""recommended_short_term_action"": ""string"", ""recommended_long_term_action"": ""string"" } ] }";

            var requestBody = new
            {
                contents = new[]
                {
                    new {
                        role = "user",
                        parts = new object[]
                        {
                            new { text = prompt },
                            new { inline_data = new { mime_type = "image/jpeg", data = imageBase64 } }
                        }
                    }
                }
            };

            var requestJson = JsonSerializer.Serialize(requestBody);
            var apiKey = _config["Gemini:ApiKey"];

            var request = new HttpRequestMessage(HttpMethod.Post,
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}")
            {
                Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
            };

            var response = await _http.SendAsync(request);
            var responseJson = await response.Content.ReadAsStringAsync();

            // Extract Gemini's JSON string from the response
            using var doc = JsonDocument.Parse(responseJson);
            var geminiText = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            // Clean possible Markdown fences or junk
            if (!string.IsNullOrEmpty(geminiText))
            {
                geminiText = geminiText.Trim();

                if (geminiText.StartsWith("```"))
                {
                    int firstNewline = geminiText.IndexOf('\n');
                    int lastFence = geminiText.LastIndexOf("```");
                    if (firstNewline >= 0 && lastFence > firstNewline)
                    {
                        geminiText = geminiText.Substring(firstNewline + 1, lastFence - firstNewline - 1);
                    }
                }
            }

            // Debug: log raw + cleaned
            Console.WriteLine("RAW Gemini Output:\n" + geminiText);

            try
            {
                return JsonSerializer.Deserialize<OutputPOJO>(geminiText!);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ JSON parse failed: " + ex.Message);
                return null;
            }

        }
    }
}
