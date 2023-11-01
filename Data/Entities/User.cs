using Data.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;


namespace Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public Guid? roleID { get; set; }
        [ForeignKey("roleID")]
        public virtual Role? Role { get; set; }

        public Guid? teamId { get; set; }
        [ForeignKey("teamId")]
        public virtual Team? Team { get; set; }

        public Guid? groupId { get; set; }
        [ForeignKey("groupId")]
        public virtual Group? Group { get; set; }

        public string fullName { get; set; } = null!;
        public string? address { get; set; }
        public string? image { get; set; }
        public string? skill { get; set; } 
        public DateTime dob { get; set; }
        public Gender gender { get; set; } 
        public bool banStatus { get; set; }
             
        //public virtual List<Order> Orders { get; set; } = new();
        public virtual List<Order> OrdersAssignTo { get; set; } = new();
        public virtual List<Order> OrdersCreatedBy { get; set; } = new();
    }
}
