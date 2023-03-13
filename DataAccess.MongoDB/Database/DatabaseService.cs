using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.MongoDB;
public class DatabaseService
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    public readonly UserRepository Users;

    public DatabaseService(string connectionString, string databaseName)
    {
        _client = new MongoClient(connectionString);
        _database = _client.GetDatabase(databaseName);
        Users = new UserRepository(_database);
    }
}
