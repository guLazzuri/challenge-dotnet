using System.ComponentModel.DataAnnotations;

namespace challenge.Domain.Entity
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }


    }
}
