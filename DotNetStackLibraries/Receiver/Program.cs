using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.Worker.Receiver
{
    /// <summary>
    /// 无论启动多少个接受者，消息队列都是同一个
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Receiver Start");
            var connFac = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "jjj",
                Password = "jjj"
            };
            using (var conn = connFac.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                //队列名一定要与Sender的相同
                var queueName = args.FirstOrDefault() ?? "queue1";
                channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false
                );

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    Console.WriteLine($"接收到消息：{Encoding.UTF8.GetString(e.Body)}");
                    channel.BasicAck(e.DeliveryTag, true);
                };
                channel.BasicConsume(
                    queue: queueName,
                    //是否启用自动命令正确应答
                    autoAck: false,
                    consumer: consumer
                );
                Console.ReadKey();
            }
        }
    }
}
