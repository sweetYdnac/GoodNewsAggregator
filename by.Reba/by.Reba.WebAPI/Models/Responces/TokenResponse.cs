namespace by.Reba.WebAPI.Models.Responces
{
    /// <summary>
    /// Model for returning Jwt token from api
    /// </summary>
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string Role { get; set; }
        public Guid UserId { get; set; }
        public DateTime TokenExpiration { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
