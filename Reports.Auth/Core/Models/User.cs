using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Reports.Auth.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new Collection<UserRole>();
    }
}
