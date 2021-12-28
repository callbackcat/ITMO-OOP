namespace Reports.Auth.Core.Security.Hashing
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyHashedPassword(string providedPassword, string hashedPassword);
    }
}