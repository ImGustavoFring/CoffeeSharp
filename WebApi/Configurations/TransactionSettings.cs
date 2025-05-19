namespace WebApi.Configurations
{
    public class TransactionSettings
    {
        public long PendingStatus { get; set; }
        public long CompletedStatus { get; set; }
        public long CancelledStatus { get; set; }
    }
}
