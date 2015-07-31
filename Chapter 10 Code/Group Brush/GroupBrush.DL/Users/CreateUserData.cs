using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.DL.Users
{
    public class CreateUserData : ICreateUserData
    {
        private string _connectionString;
        public CreateUserData(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int? CreateUser(string userName, string password)
        {
            int? returnValue = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "dbo.CreateUser";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter prmReturnValue = new SqlParameter("@ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                SqlParameter prmName = new SqlParameter("@Name", SqlDbType.NVarChar, 100)
                {
                    Direction =
                        ParameterDirection.Input,
                    Value = userName
                };
                SqlParameter prmPassword = new SqlParameter("@Password", SqlDbType.NVarChar, 255)
                {
                    Direction = ParameterDirection.Input,
                    Value = password
                };
                SqlParameter prmUserId = new SqlParameter("@UserId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(prmReturnValue);
                command.Parameters.Add(prmName);
                command.Parameters.Add(prmPassword);
                command.Parameters.Add(prmUserId);
                connection.Open();
                command.ExecuteNonQuery();
                if (prmReturnValue != null && prmReturnValue.Value != DBNull.Value && prmReturnValue.
                Value is int && (int)prmReturnValue.Value == 0)
                {
                    if (prmUserId != null && prmUserId.Value != DBNull.Value && prmUserId.Value is int)
                    {
                        returnValue = (int)prmUserId.Value;
                    }
                    else
                    {
                        returnValue = -1;
                    }
                }
            }
            return returnValue;
        }
    }
}
