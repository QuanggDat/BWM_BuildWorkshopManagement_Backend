using Data.Entities;
using Data.Enums;
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
        public string fullName { get; set; } = null!;
        public string address { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string email { get; set; } = null!;
        public string? image { get; set; }
        public DateTime dob { get; set; }
        public Gender gender { get; set; }
        public Guid? roleId { get; set; }
        public Role? Role { get; set; }
        public Guid? groupId { get; set; }
        public Group? Group { get; set; } = null!;
        public bool banStatus { get; set; }    
    }

    public class UserCreateModel
    {
        public string phoneNumber { get; set; } = null!;
        public string password { get; set; } = null!;   
        public string email { get; set; } = null!;     
        public string fullName { get; set; } = null!;
        public string address { get; set; } = null!;
        public string? image { get; set; }
        public DateTime dob { get; set; }
        public Gender gender { get; set; } 
    }
    
    public class UserUpdateModel
    {
        public Guid id { get; set; }
        public string fullName { get; set; } = null!;
        public string address { get; set; } = null!;
        public string? image { get; set; }
        public DateTime dob { get; set; }
        public Gender gender { get; set; }

    }
    public class BannedUserModel
    {
        public Guid id { get; set; }
        public bool banStatus { get; set; }

    }
    public class UserUpdatePasswordModel
    {
        public Guid id { get; set; }
        public string oldPassword { get; set; } = null!;
        public string newPassword { get; set; } = null!;
    }
    public class UserUpdatePhoneModel
    {
        public Guid id { get; set; }
        public string phoneNumber { get; set; } = null!;

    }

    public class RoleModel
    {
        public Guid id { get; set; }
        public string name { get; set; } = null!;
    }

    public class UserUpdateUserRoleModel
    {
        public Guid userId { get; set; }
        public Guid roleId { get; set; }

    }

    public class LoginModel
    {
        public string phoneNumber { get; set; } = null!;
        public string password { get; set; } = null!;

    }

    public class AddWorkerToTeamModel
    {
        public Guid id { get; set; }
        public Guid teamId { get; set; }
    }

    public class RemoveWorkerFromTeamModel
    {
        public Guid id { get; set; }
        public Guid teamId { get; set; }
    }

    public class AddWorkerToGroupModel
    {
        public Guid id { get; set; }
        public Guid groupId { get; set; }
    }

    public class RemoveWorkerFromGroupModel
    {
        public Guid id { get; set; }
        public Guid groupId { get; set; }
    }

    public class AddManagerToGroupModel
    {
        public Guid id { get; set; }
        public Guid roleId { get; set; }
        public Guid groupId { get; set; }
    }

}
