using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class UserModel
    {
        public Guid id { get; set; }
        public string fullName { get; set; }
        public string address { get; set; }
        public string UserName { get; set; }
        public string email { get; set; }       
        public string? image { get; set; }
        public DateTime dob { get; set; }
        public bool gender { get; set; }

        public Guid? roleId { get; set; }
        public Role? Role { get; set; }

        public Guid? groupId { get; set; }
        public Group Group { get; set; }

        public Guid? squadId { get; set; }
        public Squad Squad { get; set; }

        public bool banStatus { get; set; }    
    }

    public class UserCreateModel
    {
        public string phoneNumber { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string fullName { get; set; } = null!;
        [Required]
        public string address { get; set; }
        public string? image { get; set; }
        public DateTime dob { get; set; }
        public bool gender { get; set; } = true;
        public bool banStatus { get; set; } = false;
    }
    
    public class UserUpdateModel
    {
        public Guid id { get; set; }
        public string fullName { get; set; } = null!;
        public string address { get; set; }
        public string? image { get; set; }
        public DateTime dob { get; set; }
        public bool gender { get; set; } = true;

    }
    public class BannedUserModel
    {
        public Guid id { get; set; }
        public bool banStatus { get; set; }

    }
    public class UserUpdatePasswordModel
    {
        public Guid id { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
    public class UserUpdatePhoneModel
    {
        public Guid id { get; set; }
        public string phoneNumber { get; set; }

    }
    public class LoginModel
    {
        public string phoneNumber { get; set; }
        public string password { get; set; }
    }

    public class AddWorkerToGroup
    {
        public Guid id { get; set; }
        public Guid groupId { get; set; }
    }
}
