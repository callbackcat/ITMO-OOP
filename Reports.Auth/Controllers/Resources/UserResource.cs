using System;
using System.Collections.Generic;

namespace Reports.Auth.Controllers.Resources
{
    public class UserResource
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}