using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.Worker.Sender
{
    /// <summary>
    /// Woker模式（一对多）：非发布-订阅模式，而是将消息顺序的发送给多个接受者，如：
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
                    //是否持久化, 队列的声明默认是存放到内存中的，如果rabbitmq重启会丢失，如果想重启之后还存在就要使队列持久化，保存到Erlang自带的Mnesia数据库中，当rabbitmq重启之后会读取该数据库
                    durable: false,
                    //设置了排外为true的队列只可以在本次的连接中被访问，也就是说只有一个Channel能访问。排外的queue在当前连接被断开的时候会自动消失（清除）无论是否设置了持久化
                    exclusive: false,
                    //是否自动删除，当最后一个消费者断开连接之后队列是否自动被删除，可以通过RabbitMQ Management，查看某个队列的消费者数量，当consumers = 0时队列就会自动删除
                    autoDelete: false
                );
                //告诉Rabbit每次只能向消费者发送一条信息,再消费者未确认之前,不再向他发送信息
                channel.BasicQos(0, 1, false);
                while (true)
                {
                    Console.WriteLine("请输入消息内容：");
                    var message = Console.ReadLine();
                    channel.BasicPublish(
                        //交换机名字
                        exchange: string.Empty,
                        //路由
                        routingKey: queueName,
                        basicProperties: null,
                        //消息内容字节数组
                        body: Encoding.UTF8.GetBytes(message)
                    );
                    Console.WriteLine("消息已成功发送");
                }
            }
        }
    }
}
