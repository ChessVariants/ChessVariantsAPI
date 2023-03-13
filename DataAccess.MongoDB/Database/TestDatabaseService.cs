using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.MongoDB;
public class TestDatabaseService : DatabaseService
{
    public TestDatabaseService(string connectionString) : base(connectionString, "Test") {}
}
