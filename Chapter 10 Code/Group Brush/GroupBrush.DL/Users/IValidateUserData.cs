using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.DL.Users
{
    public interface IValidateUserData
    {
        bool ValidateUser(string userName, string password, out int? userId);
    }
}
