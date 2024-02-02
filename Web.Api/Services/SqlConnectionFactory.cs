using Microsoft.Data.SqlClient;

namespace Web.Api.Services;

public class SqlConnectionFactory(string connectionString)
{
    public SqlConnection Create()
    {
        return new SqlConnection(connectionString);
    }
}