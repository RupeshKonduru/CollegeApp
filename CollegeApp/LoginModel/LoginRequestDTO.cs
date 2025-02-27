using System.ComponentModel.DataAnnotations;

namespace CollegeApp.LoginModel
{
    public class LoginRequestDTO
    {
        public string PolicyName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
