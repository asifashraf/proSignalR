using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.DL.Users
{
    public interface ICreateUserData
    {
        int? CreateUser(string userName, string password);
    }
}
