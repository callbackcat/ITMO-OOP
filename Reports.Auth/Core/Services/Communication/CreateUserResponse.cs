using Reports.Auth.Core.Models;

namespace Reports.Auth.Core.Services.Communication
{
    public class CreateUserResponse : BaseResponse
    {
        public User User { get; }

        public CreateUserResponse(bool success, string message, User user)
            : base(success, message)
        {
            User = user;
        }
    }
}