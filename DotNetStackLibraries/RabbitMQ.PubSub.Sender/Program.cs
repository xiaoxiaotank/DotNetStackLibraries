using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RabbitMQ.Exchange.Sender
{
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
            Console.WriteLine("Sender Start!");
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
                var exchangeName = _exchangeNameDic[type];
                channel.ExchangeDeclare(exchangeName, type.ToString().ToLower());

                while (true)
                {
                    Console.WriteLine("请输入消息内容:");
                    var message = Console.ReadLine();

                    switch (type)
                    {
                        case ExchangeType.Fanout:
                            channel.BasicPublish(
                                exchange: exchangeName,
                                routingKey: string.Empty,
                                basicProperties: null,
                                body: Encoding.UTF8.GetBytes(message)
                            );
                            break;
                        case ExchangeType.Direct:
                            channel.BasicPublish(
                                exchange: exchangeName,
                                //只推送给绑定了temp路由的receiver
                                routingKey: "temp",
                                basicProperties: null,
                                body: Encoding.UTF8.GetBytes(message)
                            );
                            break;
                        case ExchangeType.Topic:
                            channel.BasicPublish(
                                exchange: exchangeName,
                                //只推送给绑定了temp路由的receiver
                                routingKey: "temp",
                                basicProperties: null,
                                body: Encoding.UTF8.GetBytes(message)
                            );
                            break;
                    }
                    
                    Console.WriteLine("消息已成功发送");
                }
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
                    type =ExchangeType.Direct;
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

        //#：匹配0-n个字符语句
        //*：匹配一个字符语句
        //RabbitMQ中是一个以“.”分割的字符串为一个字符语句，
        //如 "topic1.*"匹配的规则以topic1开始并且"."后只有一段语句的路由  例：“topic1.aaa”或“topic1.bb”
        Topic,
    }
}
