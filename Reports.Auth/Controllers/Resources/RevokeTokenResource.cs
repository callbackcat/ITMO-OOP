using System.ComponentModel.DataAnnotations;

namespace Reports.Auth.Controllers.Resources
{
    public class RevokeTokenResource
    {
        [Required]
        public string Token { get; set; }
    }
}