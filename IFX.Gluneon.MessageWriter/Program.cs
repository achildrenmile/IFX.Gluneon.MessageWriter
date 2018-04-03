using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NATS.Client;

namespace IFX.Gluneon.MessageWriter
{
  
    public class Program
    {
        public static IConfiguration Configuration { get; set; }
        public const string MessageTopic="normalized";

        public static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables();

            Configuration = builder.Build();

            try
            {
                ConnectionFactory cf = new ConnectionFactory();
                IConnection c = cf.CreateConnection($"{Configuration["NATS"]}");
           

                try 
                {
                    MongoDBContext.ConnectionString = $"{Configuration["ConnectionString"]}";
                    MongoDBContext.DatabaseName = $"{Configuration["Database"]}";
                    MongoDBContext.IsSSL = $"{Configuration["UseTLSforMongo"]}"=="TRUE"?true:false;
                    MongoDBContext context = new MongoDBContext();
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("{0}\tException on configuration of the MongoDBContext: {1}", DateTime.Now.ToUniversalTime(), e.Message));
                }

                EventHandler<MsgHandlerEventArgs> WriteMessageHandler = (sender, argsMsg) =>
                {
                    try
                    {
                        var msg = ProtoBuf.Serializer.Deserialize<Message>(new System.IO.MemoryStream(argsMsg.Message.Data));//argsMsg.Message.Data);

    #if DEBUG
                        Console.WriteLine(string.Format(@"Channel: {0}\n
                                                    Publisher: {1}\n
                                                    Protocol: {2}
                                                    Name: {3}\n
                                                    Unit: {4}\n
                                                    Value: {5}\n
                                                    StringValue: {6}\n
                                                    BoolValue: {7}\n
                                                    DataValue: {8}\n
                                                    ValueSum: {9}\n
                                                    Time: {10}\n
                                                    UpdateTime: {11}\n
                                                    Link: {12}\n",
                                                        msg.Channel,
                                                        msg.Publisher,
                                                        msg.Protocol,
                                                        msg.Name,
                                                        msg.Unit,
                                                        msg.Value,
                                                        msg.StringValue,
                                                        msg.BoolValue,
                                                        msg.DataValue,
                                                        msg.ValueSum,
                                                        msg.Time,
                                                        msg.UpdateTime,
                                                        msg.Link)
                                         );
    #endif               
                        MongoDBContext dbContext = new MongoDBContext();
                        msg.Id = Guid.NewGuid();
                        dbContext.Messages.InsertOne(msg);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(string.Format("{0}\tException in Eventhanlder occured: {1}", DateTime.Now.ToUniversalTime(), e.Message));
                    }

                };

                    IAsyncSubscription s2 = c.SubscribeAsync(MessageTopic, WriteMessageHandler);
                }
            catch(Exception e)
            {
                Console.WriteLine(string.Format("{0}\tNATS exception occured: {1}", DateTime.Now.ToUniversalTime(), e.Message));
            }
           
        }
    }
}
