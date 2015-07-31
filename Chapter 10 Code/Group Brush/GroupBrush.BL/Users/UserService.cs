using GroupBrush.BL.Storage;
using GroupBrush.DL.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.BL.Users
{
    public class UserService : IUserService
    {
        ICreateUserData _createUserData;
        IValidateUserData _validateUserData;
        IGetUserNameFromIdData _getUserNameFromIdData;
        IMemStorage _memStorage;
        public UserService(ICreateUserData createUserData, IValidateUserData validateUserData,
        IGetUserNameFromIdData getUserNameFromIdData, IMemStorage memStorage)
        {
            _createUserData = createUserData;
            _validateUserData = validateUserData;
            _getUserNameFromIdData = getUserNameFromIdData;
            _memStorage = memStorage;
        }
        public int? CreateAccount(string userName, string password)
        {
            return _createUserData.CreateUser(userName, password);
        }
        public bool ValidateUserLogin(string userName, string password, out int? userId)
        {
            return _validateUserData.ValidateUser(userName, password, out userId);
        }
        public string GetUserNameFromId(int id)
        {
            string userName = _memStorage.GetUserName(id);
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = _getUserNameFromIdData.GetUserName(id);
                if (!string.IsNullOrWhiteSpace(userName))
                {
                    _memStorage.StoreUserName(id, userName);
                }
            }
            return userName;
        }
    }
}
