using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.Producer
{
    public class Send
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

                string message = "Hello World!";

                var body = Encoding.UTF8.GetBytes(message);

                //Send the written message (message in bytes)
                channel.BasicPublish(
                    exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body
                    );

                Console.WriteLine($"[x] Sent {message}");
            }

            Console.WriteLine("Press [backspace] to exit.");
            Console.ReadKey();
        }
    }
}
