using System.ComponentModel.DataAnnotations;

namespace GiveOrTake.BackEnd.ViewModels
{
    public class LoginViewModel
    {
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
