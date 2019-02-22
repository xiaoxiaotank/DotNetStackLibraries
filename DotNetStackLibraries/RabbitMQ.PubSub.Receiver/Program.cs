using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitMQ.Exchange.Receiver
{
    /// <summary>
    /// 如果启动多个接受者，每个接受者都有一个属于自己的队列
    /// </summary>
    class Program
    {
        public const string ExchangeDeadName = "exchange.dead";
        public const string QueueDeadName = "queue_dead";   

        static void Main(string[] args)
        {
            //创建一个随机数,以创建不同的消息队列
            var queueIndex = new Random().Next(1, 1000);
            Console.WriteLine("Start" + queueIndex.ToString());
            Console.WriteLine($"Receiver Start: {queueIndex}");

            var connFac = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "jjj",
                Password = "jjj",
                AutomaticRecoveryEnabled = true,
            };
            using (var conn = connFac.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                var type = Sender.Program.GetExchangeType();
                //名字要与Sender中的一致
                var exchangeName = Sender.Program.ExchangeNameDic[type];
                var queueName = $"{exchangeName}_{queueIndex}";
                //为了保证该交换机被声明（如果Sender还未启动）
                channel.ExchangeDeclare(exchangeName, type.ToString().ToLower(), durable: true);
                //每一个Receiver声明一个不同名字的队列来分别接受所有消息，然后同交换机指定路由绑定后，那么该交换机指定路由的消息会传给所有绑定的队列，每个队列的消息相同
                channel.QueueDeclare(
                    queue: queueName,
                    //是否持久化（不包含消息数据，仅队列）, 队列的声明默认是存放到内存中的，如果rabbitmq重启会丢失，如果想重启之后还存在就要使队列持久化，保存到Erlang自带的Mnesia数据库中，当rabbitmq重启之后会读取该数据库
                    durable: true,
                    //设置了排外为true的队列只可以在本次的连接中被访问，也就是说只有一个Channel能访问。排外的queue在当前连接被断开的时候会自动消失（清除）无论是否设置了持久化
                    exclusive: false,
                    //是否自动删除，当最后一个消费者断开连接之后队列是否自动被删除，可以通过RabbitMQ Management，查看某个队列的消费者数量，当consumers = 0时队列就会自动删除
                    autoDelete: false,
                    arguments: new Dictionary<string, object>()
                    {
                        //队列过期时间，如果该队列出现在指定时间内未被消费，则立即删除此队列
                        [Headers.XExpires] = 10000,
                        //统一设置该队列的消息过期时间为 60s
                        [Headers.XMessageTTL] = 60000,
                        //队列中消息的个数，如不指定为无限长，当超出时，前面的消息被删除，后来的消息入队
                        //[Headers.XMaxLength] = 4,
                        //队列中消息的存储空间，单位：byte（字节），同样，超出时删除之前的消息
                        //[Headers.XMaxLengthInBytes] = 1024,
                        //因过期或达到最大长度（被后来的消息挤出）/空间被删除的消息放入到绑定了该交换机和路由的队列
                        //[Headers.XDeadLetterExchange] = "exchange_dead",
                        //[Headers.XDeadLetterRoutingKey] = "routingkey_dead",
                    }
                );
                switch (type)
                {
                    case ExchangeType.Fanout:
                        channel.QueueBind(queueName, exchangeName, string.Empty);
                        break;
                    case ExchangeType.Direct:
                        //receiver可以声明多个路由，需要包含Sender中的路由才可接收其消息
                        var routeKeys = new[] { "temp","temp2", "temp3" };
                        foreach (var routeKey in routeKeys)
                        {
                            channel.QueueBind(queueName, exchangeName, routeKey);
                        }
                        break;
                    case ExchangeType.Topic:
                        channel.QueueBind(queueName, exchangeName, "temp.#");
                        break;
                }
                //消息队列一次只发送一条消息，直到确认应答，应答后会将该消息移除
                channel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    Console.WriteLine($"接收到消息：{Encoding.UTF8.GetString(e.Body)}");
                    //发送确认应答   multiple：是否多个消息一次性确认
                    //channel.BasicAck(e.DeliveryTag, true);
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
