using CouponUnblockQualifier.Models.Database;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;

namespace CouponUnblockQualifier.Database
{
    public class DBHelper : IDBHelper
    {
        private readonly ILogger<DBHelper> _logger;

        public DBHelper(ILogger<DBHelper> logger)
        {
            _logger = logger;
        }

        static NpgsqlConnection GetConnection(string connectionString) => new NpgsqlConnection(connectionString);
        static NpgsqlCommand GetConnectionAndCommand(string connectionString, Dictionary<string, Object>? parameters = null)
        {
            var command = new NpgsqlCommand
            {
                Connection = DBHelper.GetConnection(connectionString)
            };
            if (parameters != null)
            {
                DBHelper.AddParameters(command, parameters);
            }
            return command;
        }
        private static void AddParameters(NpgsqlCommand command, Dictionary<string, object> parameters)
        {
            foreach (var parameter in parameters)
            {
                if (parameter.Key.ToLower().Contains("status"))
                {
                    command.Parameters.AddWithValue(parameter.Key, NpgsqlTypes.NpgsqlDbType.Unknown, parameter.Value);
                }
                else
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
            }
        }
        private async Task<DataTable> ExecuteQuery(string connectionString, CommandType commandType, string commandText, Dictionary<string, Object>? parameters = null)
        {
            NpgsqlDataReader? dataReader = null;
            NpgsqlCommand? command = null;
            try
            {
                command = DBHelper.GetConnectionAndCommand(connectionString, parameters);

                command.CommandType = commandType;
                command.CommandText = commandText;

                if (command?.Connection?.State == ConnectionState.Closed)
                {
                    command.Connection?.Open();
                }

                DataSet ds = new();
                DataTable dataTable = new();

                dataReader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (dataReader != null)
                {
                    dataTable.Load(dataReader);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception in ExecuteQuery : " + ex.Message);
                return new DataTable();
            }
            finally
            {
                if (command?.Connection?.State == ConnectionState.Open)
                {
                    command.Connection?.Close();
                }
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }

        }
        
        public async Task<DataTable> ExecuteQueryGetStoreDetails(CommonRequestDto storeDetailsRequest)
        {
            NpgsqlCommand? command = null;
            try
            {
                var dataTable = await ExecuteQuery(storeDetailsRequest.TenantConnectionString,
                                   CommandType.StoredProcedure,
                                   storeDetailsRequest.CommandText).ConfigureAwait(false);
                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception in ExecuteQueryGetRewardRule : " + ex.Message);
                return new DataTable();
            }
            finally
            {
                if (command?.Connection?.State == ConnectionState.Open)
                {
                    command.Connection?.Close();
                }
            }
        }
    }
}
