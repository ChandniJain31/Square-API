using System.ComponentModel.DataAnnotations;

namespace SquaresAPI.BusinessLogic
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
