using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.Consumer
{
    public class Receive
    {
        public static void Main()
        {
            //Defining host
            var factory = new ConnectionFactory() { HostName = "localhost" };

            //Creating connection with the defined host.
            using (var connection = factory.CreateConnection())

            //Creating a fresh channel, session and model
            using (var channel = connection.CreateModel())
            {
                //Declaring a queue
                channel.QueueDeclare(
                    queue: "hello", //Name of queue
                    durable: false, //Define if this queue must survive when broker/producer is restarted
                    exclusive: false, //Define if this queue use will be limited to the declaring connection. If yes, it will be deleted when declaring connection closes
                    autoDelete: false, //Define if this queue be auto-deleted when its last consumer (if any) unsubscribes
                    arguments: null //Additional queue arguments
                );

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine($" [x] Received {message}");
                };

                channel.BasicConsume(
                    queue: "hello",
                    autoAck: true,
                    consumer: consumer
                );

                Console.WriteLine(" Press [backspace] to exit.");
                Console.ReadKey();
            }
        }
    }
}
