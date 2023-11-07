using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.MessageQueue.Service
{
    public static class Producer
    {
        public static void ProduceMessage(string topic, string message)
        {
            var config = new ProducerConfig()
            {
                BootstrapServers = "kafka.wyvernpserver.tech:9092",
            };
            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                p.Produce(topic, new Message<Null, string> { Value = message });
                p.Flush(TimeSpan.FromSeconds(10));
            }
        }
    }
}
