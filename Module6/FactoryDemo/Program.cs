/* Author: Dank Mitchell
 * Date: 18 Nov 2024
 * Assignment: PA6-2 Task 2
 */

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace FactoryDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"\nFactory Model Example");

            // Call GetProviderFromConfiguration method
            var (provider, connectionString) = GetProviderFromConfiguration();

            // Print Provider and Connection string from GetProviderFromConfiguration method
            Console.WriteLine($"\tprovider: {provider}");
            Console.WriteLine($"\tconnection string: {connectionString}");

            // Use provider to get correct provider factory
            DbProviderFactory factory = GetDbProviderFactory(provider);


            // Now get the connection object.
            using (DbConnection connection = factory.CreateConnection())
            {
                // if statement to check if connection is null. Print error if null
                if (connection == null)
                {
                    Console.WriteLine($"Unable to create the connection object");
                    return;
                }

                // Print type of connection
                Console.WriteLine($"\n\tYour connection object is a: {connection.GetType().Name}");

                // Set Connection string to retrieved value and open
                connection.ConnectionString = connectionString;
                connection.Open();

                // Make command object using factory
                DbCommand command = factory.CreateCommand();

                // Check if command is null. If it is, print error message and return
                if (command == null)
                {
                    Console.WriteLine($"Unable to create the command object");
                    return;
                }

                // Print command object type
                Console.WriteLine($"\tYour command object is a: {command.GetType().Name}");

                // Set command with connection
                command.Connection = connection;

                // SQL query to retrieve inventory and make from AutoLot2023
                command.CommandText = "Select i.Id, m.Name From Inventory i inner join Makes m on m.Id = i.MakeId ";

                // run command and use datareader to retrieve results
                using (DbDataReader dataReader = command.ExecuteReader())
                {
                    // Print reader object type
                    Console.WriteLine($"\tYour data reader object is a: {dataReader.GetType().Name}");
                    Console.WriteLine("\n\t***** Current Inventory *****");

                    // Use while loop read each row of inventory and print
                    while (dataReader.Read())
                    {
                        Console.WriteLine($"\t-> Car #{dataReader["Id"]} is a {dataReader["Name"]}.");
                    }
                }
            }

        }
        // Method to return appropriate DbProviderFactory based on provider 
        static DbProviderFactory GetDbProviderFactory(string provider)
        {
            // Check if provider is SqlServer and return to SqlClientFactory. If not, return null.
            if (provider == "SqlServer")
            {
                return SqlClientFactory.Instance;
            }
            else return null;
        }

        // Method to retrieve provider and connectionString from config file
        static (string Provider, string ConnectionString)
            GetProviderFromConfiguration()
        {
            // Create the configuration object and load json
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings1.json", true, true)
                .Build();

            // Get name and connection string
            var providerName = config["ProviderName"];
            var connectionString = config[$"SqlServer:ConnectionString"];
            return (providerName, connectionString);
        }

    }
}

