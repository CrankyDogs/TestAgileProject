using System.ComponentModel.DataAnnotations;

namespace TestProjLarge.Entities
{
    public class login
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
