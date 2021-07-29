using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BasicHttpServer.MvcFramework;
using BattleCards.Data;

namespace BattleCards.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService()
        {
            db = new ApplicationDbContext();
        }

        public void CreateUser(string username, string password, string email)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                Role = IdentityRole.User,
                Password = ComputeHash(password)
            };

            db.Users.Add(user);
            db.SaveChanges();
        }

        public bool IsUserValid(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Username.Equals(username));
            return user != null && user.Password.Equals(ComputeHash(password));
        }

        public bool IsUsernameAvailable(string username)
        {
            return !db.Users.Any(u => u.Username.Equals(username));
        }

        public bool IsEmailAvailable(string email)
        {
            return !db.Users.Any(u => u.Username.Equals(email));
        }

        public static string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using var hash = SHA512.Create();
            var hashedInputBytes = hash.ComputeHash(bytes);
            var hashedInputStringBuilder = new StringBuilder(128);

            foreach (var b in hashedInputBytes)
            {
                hashedInputStringBuilder.Append(b.ToString("X2"));
            }

            return hashedInputStringBuilder.ToString();
        }
    }
}
