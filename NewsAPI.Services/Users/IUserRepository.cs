using NewsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAPI.Services.Users
{
    public interface IUserRepository
    {
        public List<User> GetAllUsers();
        public string GetUserName(int id);
        public User AddUser(User user);
        public bool CheckUserExsit(string email);
        public bool VerifyFormPassword(LoginDto data);
        public void SetAcessToken(User user);
    }
}
