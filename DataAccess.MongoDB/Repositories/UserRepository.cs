using DataAccess.MongoDB.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.MongoDB;
public class UserRepository : GenericRepository<User>
{
    public const string CollectionName = "Users";

    public UserRepository(IMongoDatabase database) : base(database.GetCollection<User>(CollectionName)) {}

}
