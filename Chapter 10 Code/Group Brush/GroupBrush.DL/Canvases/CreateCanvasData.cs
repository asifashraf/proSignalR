using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.DL.Canvases
{
    public class CreateCanvasData : ICreateCanvasData
    {
        private string _connectionString;
        public CreateCanvasData(string connectionString)
        {
            _connectionString = connectionString;
        }
        public Guid? CreateCanvas(string canvasName, string description)
        {
            Guid? returnValue = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "dbo.CreateCanvas";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter prmReturnValue = new SqlParameter("@ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                SqlParameter prmCanvasName = new SqlParameter("@CanvasName", SqlDbType.NVarChar,
                100) { Direction = ParameterDirection.Input, Value = canvasName };
                SqlParameter prmCanvasDescription = new SqlParameter("@CanvasDescription",
                SqlDbType.NVarChar, 255)
                {
                    Direction = ParameterDirection.Input,
                    Value =
                        description
                };
                SqlParameter prmCanvasId = new SqlParameter("@CanvasId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Output };
                command.Parameters.Add(prmReturnValue);
                command.Parameters.Add(prmCanvasName);
                command.Parameters.Add(prmCanvasDescription);
                command.Parameters.Add(prmCanvasId);
                connection.Open();
                command.ExecuteNonQuery();
                if (prmReturnValue != null && prmReturnValue.Value != DBNull.Value &&
                prmReturnValue.Value is int && (int)prmReturnValue.Value == 0)
                {
                    if (prmCanvasId != null && prmCanvasId.Value !=
                    DBNull.Value && prmCanvasId.Value is Guid)
                    {
                        returnValue = (Guid)prmCanvasId.Value;
                    }
                }
            }
            return returnValue;
        }
    }
}
