using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.DL.Users
{
    public class ValidateUserData : IValidateUserData
    {
        private string _connectionString;
        public ValidateUserData(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool ValidateUser(string userName, string password, out int? userId)
        {
            bool returnValue = false;
            userId = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "dbo.ValidateUser";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter prmName = new SqlParameter("@Name", SqlDbType.NVarChar, 100)
                {
                    Direction = ParameterDirection.Input,
                    Value = userName
                };
                SqlParameter prmPassword = new SqlParameter("@Password", SqlDbType.NVarChar, 255)
                {
                    Direction = ParameterDirection.Input,
                    Value = password
                };
                SqlParameter prmValidUser = new SqlParameter("@ValidUser", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                SqlParameter prmUserId = new SqlParameter("@UserId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(prmName);
                command.Parameters.Add(prmPassword);
                command.Parameters.Add(prmValidUser);
                command.Parameters.Add(prmUserId);
                connection.Open();
                command.ExecuteNonQuery();
                if (prmValidUser != null && prmValidUser.Value != DBNull.Value && prmValidUser.Value
                is int && (int)prmValidUser.Value == 1)
                {
                    if (prmUserId != null && prmUserId.Value != DBNull.Value && prmUserId.Value is int)
                    {
                        userId = (int)prmUserId.Value;
                        returnValue = true;
                    }
                }
            }
            return returnValue;
        }
    }
}
