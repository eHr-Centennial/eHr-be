namespace NetCore6.Bl.DTOs.User
{
    public class AuthenticateResponseDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}