using System;
using FBOLinx.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace FBOLinx.ServiceLayer.Test
{
    [TestFixture]
    public class BaseTestFixture<TSubject>
    {
        protected TSubject subject;

        protected void Arrange(Action<ServiceCollection> arrange = null)
        {
            var services = new ServiceCollection();
            ServiceConfiguration.Configure(services, GetConfiguration());
            if (arrange != null)
            {
                arrange(services);
            }
            var serviceProvider = services.BuildServiceProvider();
            subject = serviceProvider.GetRequiredService<TSubject>();
        }

        private static IConfiguration GetConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }
    }
}