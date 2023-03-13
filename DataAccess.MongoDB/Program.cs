// See https://aka.ms/new-console-template for more information
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

// For manual testing
var settings = MongoClientSettings.FromConnectionString("mongodb+srv://chessvariantsapi:<PASSWORD>@chess.yuskina.mongodb.net/?retryWrites=true&w=majority");
settings.ServerApi = new ServerApi(ServerApiVersion.V1);
var client = new MongoClient(settings);
var database = client.GetDatabase("Test");
