using NewsAPI.DataAccess;
using NewsAPI.Models;
using System.Security.Cryptography;

namespace NewsAPI.Services.Users
{
    public class UserSqlServerService : IUserRepository
    {
        private readonly LoginDbContext _context = new LoginDbContext();

        // ---------------------------Get Users--------------------------------
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
        // ---------------------------Get User Name--------------------------------

        public string GetUserName(int id)
        {
            return _context.Users.Where
                (t => t.Id == id).Select(p => p.Name).First();
        }
        // ---------------------------Add User--------------------------------
        public User AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return _context.Users.Find(user.Id);

        }
        // ---------------------------Check User Exist--------------------------------
        public bool CheckUserExsit(string email)
        {
            if (_context.Users.Any(t => t.Email == email)) { return true; }
            return false;
        }
        // ---------------------------Verify Password--------------------------------
        public bool VerifyFormPassword(LoginDto data)
        {
          
            byte[] passwordHash = _context.Users.Where
                (t => t.Email == data.Email).Select(p => p.PasswordHash).First();
            byte[] passwordSalt = _context.Users.Where
                (t => t.Email == data.Email).Select(p => p.PasswordSalt).First();
            
            return getPassword(data.Password, passwordHash, passwordSalt);
           

        }
        // ---------------------------Add User AccessToken--------------------------------
        public void SetAcessToken(User user)
        {
            var newUs = new User();
            newUs =  _context.Users.Where
                (t => t.Email == user.Email).First();
            newUs.AcessToken = user.AcessToken;
            _context.SaveChanges();

        }
        // ---------------------------Verify Password Match--------------------------------
        private bool getPassword(string ps, byte[] passwordHash, byte[] passwordSalt)
        {
            using (HMACSHA512? hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(ps));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
     

    }
}
