using Dapper;
using Faucet4u.GlobalConnections.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Faucet4u.GlobalConnections
{
    public class Log
    {
        public static void Info(Guid Guid, string Message, string ExceptionMessage = null, string IPinString = null, string Username = null)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Other.SQLConnectionString))
                {
                    const string Query = "Insert into Logs(Guid, DateTime, IP, Username, Type, Message, Exception) values (@Guid, @DateTime, @IP, @Username, @Type, @Message, @Exception)";

                    DynamicParameters Parameters = new DynamicParameters();

                    Parameters.Add("@Guid", Guid, DbType.Guid, ParameterDirection.Input);
                    Parameters.Add("@DateTime", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
                    Parameters.Add("@IP", IPinString, DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Username", Username, DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Type", "Info", DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Message", Message, DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Exception", ExceptionMessage, DbType.String, ParameterDirection.Input);

                    Connection.Execute(Query, Parameters, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(Other.secondaryLogsPath, ex.ToString() + Environment.NewLine);
            }


        }
        public static void Hack(Guid Guid, string Message, string ExceptionMessage = null, string IPinString = null, string Username = null)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Other.SQLConnectionString))
                {
                    const string Query = "Insert into Logs(Guid, DateTime, IP, Username, Type, Message, Exception) values (@Guid, @DateTime, @IP, @Username, @Type, @Message, @Exception)";

                    DynamicParameters Parameters = new DynamicParameters();

                    Parameters.Add("@Guid", Guid, DbType.Guid, ParameterDirection.Input);
                    Parameters.Add("@DateTime", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
                    Parameters.Add("@IP", IPinString, DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Username", Username, DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Type", "Hack", DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Message", Message, DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Exception", ExceptionMessage, DbType.String, ParameterDirection.Input);

                    Connection.Execute(Query, Parameters, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(Other.secondaryLogsPath, ex.ToString() + Environment.NewLine);
            }


        }
        public static void Cheat(Guid Guid, string Message, string ExceptionMessage = null, string IPinString = null, string Username = null)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Other.SQLConnectionString))
                {
                    const string Query = "Insert into Logs(Guid, DateTime, IP, Username, Type, Message, Exception) values (@Guid, @DateTime, @IP, @Username, @Type, @Message, @Exception)";

                    DynamicParameters Parameters = new DynamicParameters();

                    Parameters.Add("@Guid", Guid, DbType.Guid, ParameterDirection.Input);
                    Parameters.Add("@DateTime", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
                    Parameters.Add("@IP", IPinString, DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Username", Username, DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Type", "Cheat", DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Message", Message, DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Exception", ExceptionMessage, DbType.String, ParameterDirection.Input);

                    Connection.Execute(Query, Parameters, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(Other.secondaryLogsPath, ex.ToString() + Environment.NewLine);
            }


        }
        public static void Error(Guid Guid, string Message, string ExceptionMessage = null, string IPinString = null, string Username = null)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Other.SQLConnectionString))
                {
                    const string Query = "Insert into Logs(Guid, DateTime, IP, Username, Type, Message, Exception) values (@Guid, @DateTime, @IP, @Username, @Type, @Message, @Exception)";

                    DynamicParameters Parameters = new DynamicParameters();

                    Parameters.Add("@Guid", Guid, DbType.Guid, ParameterDirection.Input);
                    Parameters.Add("@DateTime", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
                    Parameters.Add("@IP", IPinString, DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Username", Username, DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Type", "Error", DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Message", Message, DbType.String, ParameterDirection.Input);
                    Parameters.Add("@Exception", ExceptionMessage, DbType.String, ParameterDirection.Input);

                    Connection.Execute(Query, Parameters, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(Other.secondaryLogsPath, ex.ToString() + Environment.NewLine);
            }


        }
    }
}
