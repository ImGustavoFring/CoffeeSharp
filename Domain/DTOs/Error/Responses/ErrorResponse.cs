namespace Domain.DTOs.Error.Responses
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        public required string Title { get; set; }
        public required string Detail { get; set; }
        public required string TraceId { get; set; }
        public string? Instance { get; set; }
        public string? Method { get; set; }
        public string? Query { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
