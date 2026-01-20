using System.Data;
using System.Data.SqlClient;

namespace HorusV2.Infrastructure.Data.Relational;

public class RelationalDatabaseContext
{
    public RelationalDatabaseContext(string connectionString)
    {
        DatabaseConnection = new SqlConnection(connectionString);
    }

    public IDbConnection DatabaseConnection { get; private set; }

}