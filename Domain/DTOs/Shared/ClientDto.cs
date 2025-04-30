namespace Domain.DTOs.Shared
{
    public class ClientDto
    {
        public long Id { get; set; }
        public string TelegramId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}