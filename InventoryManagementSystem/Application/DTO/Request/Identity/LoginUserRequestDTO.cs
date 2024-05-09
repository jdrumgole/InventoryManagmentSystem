using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Resquest.Identity
{
    public class LoginUserRequestDTO
    {
        //email requirements
        [EmailAddress]
        [RegularExpression("[^@ \\t\\r\\n]+@[^@ \\t\\r\\n]+\\.[^@ \\t\\r\\n]+", ErrorMessage = "Email not valid")]
        public string Email { get; set; }
        //passwords requirements
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=)(?=.*?[#?!@$ %^&*-])", ErrorMessage = "Your Password is not correct ")]
        [MinLength(8), MaxLength(100)]
        public string Password { get; set; }
    }
}
