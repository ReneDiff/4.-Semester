using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyBGList.DTO
{
    public class DomainDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression("^[a-zA-Z]+$",ErrorMessage = "Value must contain only letters (no spaces, digits, or other chars)")]
        public string? Name { get; set; }
    }
}
