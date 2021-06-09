using FBOLinx.Job.Base;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FBOLinx.Job
{
    class Program
    {
        static async Task Main(string[] args)
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
                Console.WriteLine("  -e\tRun Engagement Emails");
            }

            JobHandler jobHandler = new JobHandler(config);
            await jobHandler.RunJob(args[0]);
        }
    }
}
