using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.NewTask
{
    public class NewTask
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "task_queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                string message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "task_queue",
                    basicProperties: properties,
                    body: body
                    );

                Console.WriteLine($"[x] Sent {message}");
            }

            Console.WriteLine("Press [backspace] to exit.");
            Console.ReadKey();
        }

        private static string GetMessage(string[] args) =>
            (args.Length > 0) ? string.Join(" ", args) : "Hello World!";
    }
}
