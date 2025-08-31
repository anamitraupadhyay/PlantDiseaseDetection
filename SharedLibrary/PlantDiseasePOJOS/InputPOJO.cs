using System.ComponentModel.DataAnnotations;

namespace PlantDiseasePOJOS
{
    public class InputPOJO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Foldername { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string Filename { get; set; } = string.Empty;


        public InputPOJO()
        {
            Foldername = string.Empty;
            Filename = string.Empty;
        }
    }
}
