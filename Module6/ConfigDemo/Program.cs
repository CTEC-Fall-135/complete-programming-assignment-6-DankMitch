/* Author: Dank Mitchell
 * Date: 18 Nov 2024
 * Assignment: PA6-2 Task 1
 */

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


namespace ConfigDemo
{
    internal class Program
    {
        // Call GetProviderFromConfiguration and print out the two returns
        static void Main(string[] args)
        {
            
            var (provider, connectionString) = GetProviderFromConfiguration();

            Console.WriteLine($"Provider info");
            Console.WriteLine($"\tprovider: {provider}");
            Console.WriteLine($"\tconnection string: {connectionString}");
        }

        static (string Provider, string ConnectionString)
            GetProviderFromConfiguration()
        {
            // Create the configuration object
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            // Get name and connection string
            var providerName = config["ProviderName"];
            var connectionString = config[$"SqlServer:ConnectionString"];
            return(providerName, connectionString);
        }
    }
}
