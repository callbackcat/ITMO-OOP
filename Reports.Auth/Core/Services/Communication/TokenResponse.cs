using Reports.Auth.Core.Security.Tokens;

namespace Reports.Auth.Core.Services.Communication
{
    public class TokenResponse : BaseResponse
    {
        public AccessToken Token { get; set; }

        public TokenResponse(bool success, string message, AccessToken token)
            : base(success, message)
        {
            Token = token;
        }
    }
}