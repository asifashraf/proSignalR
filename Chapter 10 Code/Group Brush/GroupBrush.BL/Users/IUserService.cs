using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.BL.Users
{
    public interface IUserService
    {
        int? CreateAccount(string userName, string password);
        bool ValidateUserLogin(string userName, string password, out int? userId);
        string GetUserNameFromId(int id);
    }
}
