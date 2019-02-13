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
        private static IReadOnlyDictionary<ExchangeType, string> _exchangeNameDic
           = new Dictionary<ExchangeType, string>
           {
               [ExchangeType.Fanout] = "exchange_fanout",
               [ExchangeType.Direct] = "exchange_direct",
               [ExchangeType.Topic] = "exchange_topic"
           };

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
                Password = "jjj"
            };
            using (var conn = connFac.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                var type = GetExchangeType();
                //名字要与Sender中的一致
                var exchangeName = _exchangeNameDic[type];
                var queueName = $"{exchangeName}_{queueIndex}";
                channel.ExchangeDeclare(exchangeName, type.ToString().ToLower());
                channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false
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
                channel.BasicQos(0, 1, false);

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

        static ExchangeType GetExchangeType()
        {
            Console.WriteLine("请选择交换机模式:");
            Console.WriteLine("1.发布订阅模式(fanout)\n" +
                "2.路由模式(direct)\n" +
                "3.通配符模式(topic)");

            ExchangeType type;
            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    type = ExchangeType.Fanout;
                    break;
                case '2':
                    type = ExchangeType.Direct;
                    break;
                case '3':
                    type = ExchangeType.Topic;
                    break;
                default:
                    throw new ArgumentException();
            }
            Console.WriteLine("\n模式选择成功！");
            return type;
        }
    }


    enum ExchangeType
    {
        Fanout,
        Direct,
        Topic,
    }
}
