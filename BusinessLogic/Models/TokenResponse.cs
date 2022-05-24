namespace SquaresAPI.BusinessLogic
{
    public class TokenResponse
    {
        public string username { get; set; }
        public DateTime expiration { get; set; }
        public string token { get; set; }
        public string refresh_token { get; set; }

        public TokenResponse() { }

        public TokenResponse(string Username,DateTime expires, string jwtToken, string refreshtoken)
        {
            this.username = Username;
            this.expiration = expires;
            this.token = jwtToken;
            this.refresh_token = refreshtoken;
        }
    }
}
