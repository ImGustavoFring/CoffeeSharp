namespace Domain.DTOs.Log.Responses
{
    public class LogEntryResponse
    {
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string LogEvent { get; set; }
        public string Message { get; set; }
        public string MessageTemplate { get; set; }
        public string Exception { get; set; }
    }
}
