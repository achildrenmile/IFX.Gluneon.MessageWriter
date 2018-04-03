using System;
using MongoDB.Driver;

namespace IFX.Gluneon.MessageWriter
{
    public class MongoDBContext 
    { 
        public static string ConnectionString { get; set; } 
        public static string DatabaseName { get; set; } 
        public static bool IsSSL { get; set; } 
 
        private IMongoDatabase _database { get; } 
 
        public MongoDBContext() 
        { 
            try 
            { 
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString)); 
                if (IsSSL) 
                { 
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 }; 
                } 
                var mongoClient = new MongoClient(settings); 
                _database = mongoClient.GetDatabase(DatabaseName); 
            } 
            catch (Exception ex) 
            { 
                throw new Exception("Can not access to db server.", ex); 
            } 
        } 
 
        public IMongoCollection<Message> Messages 
        { 
            get 
            { 
                return _database.GetCollection<Message>("Messages"); 
            } 
        } 
    } 
}
