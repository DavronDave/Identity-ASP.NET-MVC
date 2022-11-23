using System;
using System.ComponentModel.DataAnnotations;

namespace TaskFour.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Remember me")]
        public bool RememberMe { get; set; }

        public DateTime? LoginTime { get; set; }= DateTime.Now;

        public bool IsActive { get; set; }  

    }
}
