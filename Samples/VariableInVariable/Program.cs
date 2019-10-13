using System;
using Microsoft.Extensions.Configuration;
using C9S.Configuration.Variables;

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

            configuration.ResolveVariables("${", "}");

            Console.WriteLine(configuration.GetSection("Auth:ClientID"));
        }
    }
}
