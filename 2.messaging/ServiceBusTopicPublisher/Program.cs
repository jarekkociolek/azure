using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBusTopicPublisher
{
    class Program
    {
        public static IConfigurationRoot configuration;

        static async Task Main(string[] args)
        {
            ConfigureServices(new ServiceCollection());

            var connectionString = configuration.GetConnectionString("TopicConnectionString");
            var topicName = configuration.GetValue<string>("TopicName");

            var topicClient = new TopicClient(connectionString, topicName);
            Console.WriteLine("Write a message:");
            Console.WriteLine("");
            var content = Console.ReadLine();

            while (content != "exit")
            {
                var messageToSend = new Message(Encoding.UTF8.GetBytes(content));

                try
                {
                    await topicClient.SendAsync(messageToSend);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                content = Console.ReadLine();
            }

            await topicClient.CloseAsync();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}
