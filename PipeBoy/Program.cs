using FileOps;
using FileOps.Pipe;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace PipeBoy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory());
            //.AddJsonFile("settings.json");
            //var x = new ConfigurationFactory().Get<Settings>(new FileInfo("settings.json"));
            var x = new FileOpsBuilder()
                .AddConfiguration(new FileInfo("settings.json"))
                .AddConfiguration(new FileInfo("shared.settings.json"))
                .Build();

            IFileOpsManager manager = new FileOpsManager(x);

            manager.Execute();

        }
    }
}
