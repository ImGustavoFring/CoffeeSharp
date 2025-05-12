namespace WebApi.Configurations
{
    public class JwtSettings
    {
        public string Secret { get; set; } = null!;
        public int ExpiryHours { get; set; } = 1;
    }
}
