using System;
using System.ComponentModel.DataAnnotations;

namespace Reports.Auth.Core.Models
{
    public class Role
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}