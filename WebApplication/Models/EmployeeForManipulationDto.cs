using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class EmployeeForManipulationDto
    {
        [Required(ErrorMessage = "Employee name is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the name is 30 characters")]
        public string Name { get; set; }

        [Range(18, 150, ErrorMessage = "Age is a required field and it can't be lower than 18 and greater than 150")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Employee name is a required field")]
        [MaxLength(30, ErrorMessage = "Maximum length for the position is 30 characters")]
        public string Position { get; set; }
    }
}
