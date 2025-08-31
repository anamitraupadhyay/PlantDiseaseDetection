using PlantDiseasePOJOS;

namespace Services
{
    public class PlantAnalysisService
    {
        public OutputPOJO Analyze(InputPOJO input, string filePath)
        {
            // 🔹 Later: Call Python ML model with filePath
            // For now, return dummy data
            return new OutputPOJO(new List<PlantAnalysis>
            {
                new PlantAnalysis
                {
                    PlantName = "Tomato",
                    ScientificPlantName = "Solanum lycopersicum",
                    Status = "Infected",
                    DetectionConfidencePercent = 92,
                    DiseaseName = "Early Blight",
                    DiseaseConfidencePercent = 85,
                    ScientificDiseaseName = "Alternaria solani",
                    Type = "Fungal",
                    PrimaryObservation = "Brown spots with concentric rings",
                    Severity = "High",
                    SpreadRisk = "Very High",
                    RecommendedShortTermAction = "Apply copper fungicide immediately",
                    RecommendedLongTermAction = "Use resistant varieties and crop rotation"
                }
            });
        }
    }
}
