using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Client.Requests
{
    public class AddBalanceRequest
    {
        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}