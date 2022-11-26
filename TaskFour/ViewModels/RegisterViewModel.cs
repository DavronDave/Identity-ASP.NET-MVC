using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaskFour.ViewModels
{
    public class RegisterViewModel
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Confirm password")]
        [Compare("Password",ErrorMessage ="Password and Confirmation password don't match!")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public DateTime LoginTime { get; set; }=DateTime.Now;
        public bool IsActive { get; set; }


    }
}
