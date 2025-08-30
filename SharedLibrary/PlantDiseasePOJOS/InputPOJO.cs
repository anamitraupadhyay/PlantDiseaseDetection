using System.ComponentModel.DataAnnotations;


namespace PlantDiseasePOJOS
{

    public class InputPOJO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Foldername { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string Filename { get; set; }

        /*public InputPOJO(string foldername, string filename)
        {
            Foldername = foldername;
            Filename = filename;
        }*/

        // Parameterless constructor for model binding (Blazor)
        public InputPOJO()
        {
            Foldername = string.Empty;
            Filename = string.Empty;
        }
    }

}
