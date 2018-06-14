using System;
using C9S.Extensions.Configuration;
using Microsoft.Extensions.Configuration;

namespace VariableInVariable
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()    
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile($"appsettings.json", optional: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            configuration.ResolveVariables();

            Console.WriteLine(configuration.GetSection("Auth:ClientID"));
        }
    }
}
