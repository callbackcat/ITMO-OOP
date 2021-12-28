using System;

namespace Reports.Auth.Core.Security.Tokens
{
    public class AccessToken : JsonWebToken
    {
        public RefreshToken RefreshToken { get; set; }

        public AccessToken(string token, long expiration, RefreshToken refreshToken)
            : base(token, expiration)
        {
            _ = refreshToken ?? throw new ArgumentException("Specify a valid refresh token");
            RefreshToken = refreshToken;
        }
    }
}