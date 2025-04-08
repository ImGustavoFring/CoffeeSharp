using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
    public class CancelTransactionRequest
    {
        [Required(ErrorMessage = "TransactionId is required.")]
        public long TransactionId { get; set; }
    }
}