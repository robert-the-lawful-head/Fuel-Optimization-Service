using FBOLinx.Job.Base;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace FBOLinx.Job
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();

            if (args.Length == 0)
            {
                Console.WriteLine("No job command passed.");
                Console.WriteLine();
                Console.WriteLine("These are available job commands");
                Console.WriteLine();
                Console.WriteLine("  -a\tRun Airport Watch");
            }

            JobHandler jobHandler = new JobHandler(config);
            jobHandler.RunJob(args[0]);
        }
    }
}
