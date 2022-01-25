namespace NetCore6.Core.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public int ExpiresInMinutes { get; set; }
    }
}