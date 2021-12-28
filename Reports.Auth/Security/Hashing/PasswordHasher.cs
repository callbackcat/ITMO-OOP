using Reports.Auth.Core.Security.Hashing;

namespace Reports.Auth.Security.Hashing
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password);

        public bool VerifyHashedPassword(string providedPassword, string hashedPassword) =>
            BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
    }
}