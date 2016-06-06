using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Text;

class Receive
{
    public static void Main()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "admin",
            Password = "admin",
            Port = 5672
        };
        var log = new LoggerConfiguration()
                .WriteTo.MongoDB("mongodb://localhost:27017/logs")
                .CreateLogger();

        var _client = new MongoClient();
        var _db = _client.GetDatabase("Database");
        var collection = _db.GetCollection<BsonDocument>("data");
       


        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "serilog-logs",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var document = BsonSerializer.Deserialize<BsonDocument>(message);
                    collection.InsertOne(document);
                    channel.BasicAck(ea.DeliveryTag, false);

                    //Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "serilog-logs",
                                     noAck: false,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}