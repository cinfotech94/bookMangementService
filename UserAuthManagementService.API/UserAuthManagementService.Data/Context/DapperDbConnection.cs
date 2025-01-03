using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuthManagementService.Data.context;
public class DappperDbConnection
{
    private readonly string _connectionString;

    public DappperDbConnection(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
    public IDbConnection CreateMasterConnection()
    {
        return new NpgsqlConnection(_connectionString); // Replace with your DB provider
    }
}
