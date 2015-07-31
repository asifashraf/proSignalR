using GroupBrush.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.DL.Canvases
{
    public class GetCanvasDescriptionData : IGetCanvasDescriptionData
    {
        private string _connectionString;
        public GetCanvasDescriptionData(string connectionString)
        {
            _connectionString = connectionString;
        }
        public CanvasDescription GetCanvasDescription(Guid canvasId)
        {
            CanvasDescription returnValue = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "dbo.GetCanvasDescription";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter prmCanvasId = new SqlParameter("@CanvasId", SqlDbType.UniqueIdentifier) { Direction = ParameterDirection.Input, Value = canvasId };
                SqlParameter prmCanvasName = new SqlParameter("@CanvasName", SqlDbType.NVarChar,
                100) { Direction = ParameterDirection.Output };
                SqlParameter prmCanvasDescription = new SqlParameter("@CanvasDescription",
                SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
                command.Parameters.Add(prmCanvasId);
                command.Parameters.Add(prmCanvasName);
                command.Parameters.Add(prmCanvasDescription);
                connection.Open();
                command.ExecuteNonQuery();
                returnValue = new CanvasDescription();
                if (prmCanvasName != null && prmCanvasName.Value != DBNull.Value && prmCanvasName.
                Value is string)
                {
                    returnValue.Name = (string)prmCanvasName.Value;
                }
                if (prmCanvasDescription != null && prmCanvasDescription.Value != DBNull.Value &&
                prmCanvasDescription.Value is string)
                {
                    returnValue.Description = (string)prmCanvasDescription.Value;
                }
            }
            return returnValue;
        }
    }
}
