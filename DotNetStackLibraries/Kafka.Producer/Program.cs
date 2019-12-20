using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace Kafka.Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ProducerConfig() { BootstrapServers = "localhost:9092" };

            using(var p = new ProducerBuilder<Null, string>(config).Build())
            {
                while (true)
                {
                    try
                    {
                        var dr = await p.ProduceAsync("test-topic", new Message<Null, string> { Value = "test" });
                        var dr1 = await p.ProduceAsync("test-topic1", new Message<Null, string> { Value = "test1" });
                        Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'{Environment.NewLine}Delivered '{dr1.Value}' to '{dr1.TopicPartitionOffset}'");
                    }
                    catch(ProduceException<Null, string> ex)
                    {
                        Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
                    }
                    Console.ReadKey();
                }
            }
        }
    }
}
