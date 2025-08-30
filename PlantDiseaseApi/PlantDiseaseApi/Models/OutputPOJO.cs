namespace PlantDiseaseApi.POJOs;

using System.Text.Json.Serialization;


public class OutputPOJO(List<PlantAnalysis> plantAnalysis)
{
    [JsonPropertyName("plant_analysis")]
    public List<PlantAnalysis> PlantAnalysis { get; set; } = plantAnalysis;
}

public class PlantAnalysis
{
    [JsonPropertyName("plant_name")]
    public required string PlantName { get; set; }

    [JsonPropertyName("detection_confidence_percent")]
    public int DetectionConfidencePercent { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("primary_observation")]
    public required string PrimaryObservation { get; set; }

    [JsonPropertyName("disease_name")]
    public required string DiseaseName { get; set; }

    [JsonPropertyName("disease_confidence_percent")]
    public int DiseaseConfidencePercent { get; set; }

    [JsonPropertyName("scientific_plant_name")]
    public required string ScientificPlantName { get; set; }

    [JsonPropertyName("scientific_disease_name")]
    public required string ScientificDiseaseName { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("severity")]
    public required string Severity { get; set; }

    [JsonPropertyName("spread_risk")]
    public required string SpreadRisk { get; set; }

    [JsonPropertyName("recommended_short_term_action")]
    public required string RecommendedShortTermAction { get; set; }

    [JsonPropertyName("recommended_long_term_action")]
    public required string RecommendedLongTermAction { get; set; }
}
