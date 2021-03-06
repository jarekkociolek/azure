using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceBusQueuePublisher
{
    class Program
    {
        public static IConfigurationRoot configuration;

        static async Task Main(string[] args)
        {
            ConfigureServices(new ServiceCollection());

            var connectionString = configuration.GetConnectionString("QueueConnectionString");
            var queueName = configuration.GetValue<string>("QueueName");

            var queueClient = new QueueClient(connectionString, queueName);
            Console.WriteLine("Write a message:");
            Console.WriteLine("");
            var content = Console.ReadLine();

            while (content != "exit")
            {
                var messageToSend = new Message(Encoding.UTF8.GetBytes(content));

                try
                {
                    await queueClient.SendAsync(messageToSend);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                content = Console.ReadLine();
            }

            await queueClient.CloseAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args);

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
        }
    }
}
