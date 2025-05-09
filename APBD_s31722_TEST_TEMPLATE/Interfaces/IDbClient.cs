using System.Data;
using APBD_s31722_TEST_TEMPLATE.DataLayer;
using APBD_s31722_TEST_TEMPLATE.DataLayer.Models;
using Microsoft.Data.SqlClient;

namespace APBD_s31722_TEST_TEMPLATE.Interfaces;

public interface IDbClient
{
    IAsyncEnumerable<T> ReadDataAsync<T>(
        string query, 
        Func<SqlDataReader, T> map,
        Dictionary<string, object> parameters = null);
    
    Task<T> ReadScalarAsync<T>(
        string query, 
        Dictionary<string, object> parameters = null,
        CommandType commandType = CommandType.Text);
    
    Task<int?> ReadScalarAsync(
        string query, 
        Dictionary<string, object> parameters = null);

    Task<int> ExecuteNonQueriesAsTransactionAsync(List<CommandConfig> commands);

    Task<int> ExecuteNonQueryAsync(string query, Dictionary<string, object> parameters = null);
    
    Task<int> ExecuteNonQueryInTransactionAsync<T>(string query,Dictionary<string,object> parameters = null);


}