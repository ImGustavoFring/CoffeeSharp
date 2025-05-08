using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class CreateClientRequest
    {
        [Required(ErrorMessage = "TelegramId is required.")]
        public string TelegramId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
    }
}