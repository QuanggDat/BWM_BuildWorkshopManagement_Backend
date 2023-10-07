using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string fullName { get; set; } = null!;
        public string? address { get; set; }
        public string? image { get; set; }
        public string? skill { get; set; } 
        public DateTime dob { get; set; }
        public bool gender { get; set; }
        public bool banStatus { get; set; }
<<<<<<< HEAD

        [ForeignKey("squadId")]
        public Guid squadId { get; set; }
        public Squad squad { get; set; }

        public Guid groupId { get; set; }
=======
>>>>>>> QuangDat
        [ForeignKey("groupId")]
        public Guid groupId { get; set; }
        [ForeignKey("roleID")]
        public Guid? roleID { get; set; }  
        public virtual Role? Role { get; set; }  

        //public virtual List<ManagerTask> ManagerTasks { get; set; } = new();
        //public virtual List<ManagerTask> CreateBy { get; set; } = new();
        public virtual List<Order> Orders { get; set; } = new();
    }
}
