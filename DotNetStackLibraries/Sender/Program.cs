using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.Worker.Sender
{
    /// <summary>
    /// Woker模式（一对多）：并不是发布-订阅模式，而是将消息顺序的发送给多个接受者，如：
    ///     Sender发送：1，2，3，4，5
    ///     Receiver1接收：1，3，5
    ///     Receiver2接收：2，4
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sender Start");
            var connFac = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "jjj",
                Password = "jjj",
                //默认"/"
                //VirtualHost = ConnectionFactory.DefaultVHost
            };
            using(var conn = connFac.CreateConnection())
            using(var channel = conn.CreateModel())
            {
                var queueName = args.FirstOrDefault() ?? "queue1";
                channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false
                );
                //告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
                channel.BasicQos(0, 1, false);
                while (true)
                {
                    Console.WriteLine("请输入消息内容：");
                    var message = Console.ReadLine();
                    channel.BasicPublish(
                        exchange: string.Empty,
                        routingKey: queueName,
                        basicProperties: null,
                        body: Encoding.UTF8.GetBytes(message)
                    );
                    Console.WriteLine("消息已成功发送");
                }
            }
        }
    }
}
