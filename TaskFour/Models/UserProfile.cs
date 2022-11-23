using System;

namespace TaskFour.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LoginTime { get; set; }
        public User User { get; set; } 
    }
}
