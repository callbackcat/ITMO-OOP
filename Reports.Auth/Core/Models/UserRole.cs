using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reports.Auth.Core.Models
{
    [Table("UserRoles")]
    public class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}