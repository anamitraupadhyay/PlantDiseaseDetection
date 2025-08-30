namespace PlantDiseaseApi.POJOs
{

    public class InputPOJO(string Foldername, string Filename)
    {
        public string Foldername { get; set; } = Foldername;
        public string Filename { get; set; } = Filename;
    }
}
