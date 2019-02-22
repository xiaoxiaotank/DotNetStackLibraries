using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Exchange.Sender
{
    public class Program
    {
        public static readonly IReadOnlyDictionary<string, string> ExchangeNameDic
            = new Dictionary<string, string>
            {
                [ExchangeType.Fanout] = "exchange.fanout",
                [ExchangeType.Direct] = "exchange.direct",
                [ExchangeType.Topic] = "exchange.topic"
            };

        static void Main(string[] args)
        {
            Console.WriteLine("Sender Start!");
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
                var type = GetExchangeType();
                var exchangeName = ExchangeNameDic[type];
                channel.ExchangeDeclare(
                    exchange: exchangeName, 
                    type: type.ToString().ToLower(),
                    durable: true);
                #region 为单条数据设置消息持久化，会降低性能
                var basicProperties = channel.CreateBasicProperties();
                //1：非持久化 2：持久化
                basicProperties.DeliveryMode = 2;
                //过期时间，超时则删除，如果队列也设置了超时，则以队列的为准    单位：毫秒
                basicProperties.Expiration = "100000";
                #endregion

                while (true)
                {
                    channel.ConfirmSelect();
                    Console.WriteLine("请输入消息内容:");
                    var message = Console.ReadLine();

                    switch (type)
                    {
                        case ExchangeType.Fanout:
                            channel.BasicPublish(
                                //交换机名
                                exchange: exchangeName,
                                //路由
                                routingKey: string.Empty,
                                basicProperties: null,
                                //basicProperties: basicProperties,
                                //内容字节数组
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
                    //当生产者发布消息到RabbitMQ中，生产者需要知道是否真的已经发送到RabbitMQ中，需要RabbitMQ告诉生产者。
                    //Confirm机制（性能更好）
                    //  channel.confirmSelect();
                    //  channel.waitForConfirms();
                    //事务机制（性能差）
                    //  channel.txSelect();
                    //  channel.txCommit();
                    //  channel.txRollback();
                    Task.Run(() =>
                    {
                        var isPublished = channel.WaitForConfirms();
                        if (isPublished)
                        {
                            Console.WriteLine("消息已成功发送！");
                        }
                        else
                        {
                            Console.WriteLine("消息发送失败！");
                        }
                    });
                }
            }
        }

        public static string GetExchangeType()
        {
            Console.WriteLine("请选择交换机模式:");
            Console.WriteLine("1.发布订阅模式(fanout)\n" +
                "2.路由模式(direct)\n" +
                "3.通配符模式(topic)");

            string type = string.Empty;
            switch (Console.ReadKey().KeyChar)
            {
                case '1':
                    type = ExchangeType.Fanout;
                    break;
                case '2':
                    type =ExchangeType.Direct;
                    break;
                case '3':
                    //#：匹配0-n个字符语句
                    //*：匹配一个字符语句
                    //RabbitMQ中是一个以“.”分割的字符串为一个字符语句，
                    //如 "topic1.*"匹配的规则以topic1开始并且"."后只有一段语句的路由  例：“topic1.aaa”或“topic1.bb”
                    type = ExchangeType.Topic;
                    break;
                default:
                    throw new ArgumentException();
            }
            Console.WriteLine("\n模式选择成功！");
            return type;
        }
    }
}
