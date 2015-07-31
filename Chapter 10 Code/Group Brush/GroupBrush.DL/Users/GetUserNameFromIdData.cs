using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.DL.Users
{
    public class GetUserNameFromIdData : IGetUserNameFromIdData
    {
        private string _connectionString;
        public GetUserNameFromIdData(string connectionString)
        {
            _connectionString = connectionString;
        }
        public string GetUserName(int id)
        {
            string returnValue = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "dbo.GetUserName";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter prmUserId = new SqlParameter("@UserId", SqlDbType.Int)
                {
                    Direction =
                        ParameterDirection.Input,
                    Value = id
                };
                SqlParameter prmUserName = new SqlParameter("@UserName", SqlDbType.NVarChar, 100)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(prmUserName);
                command.Parameters.Add(prmUserId);
                connection.Open();
                command.ExecuteNonQuery();
                if (prmUserName != null && prmUserName.Value != DBNull.Value && prmUserName.Value
                is string)
                {
                    returnValue = (string)prmUserName.Value;
                }
            }
            return returnValue;
        }
    }
}
