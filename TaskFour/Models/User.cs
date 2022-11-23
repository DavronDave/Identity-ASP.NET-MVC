using Microsoft.AspNetCore.Identity;
using System;

namespace TaskFour.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LoginTime { get; set; }
        public bool IsActive { get; set; }
    }
}
