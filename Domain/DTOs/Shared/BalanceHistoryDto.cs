namespace Domain.DTOs.Shared
{
    public class BalanceHistoryDto
    {
        public long Id { get; set; }
        public long? ClientId { get; set; }
        public decimal Sum { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public long? BalanceHistoryStatusId { get; set; }
    }
}