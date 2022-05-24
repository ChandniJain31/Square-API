namespace SquaresAPI.BusinessLogic
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public int ExpirationTimeInMinutes { get; set; }
    }
}
