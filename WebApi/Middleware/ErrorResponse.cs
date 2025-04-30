namespace WebApi.Middleware
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        public required string Title { get; set; }
        public required string Detail { get; set; }
        public required string TraceId { get; set; }
    }
}
