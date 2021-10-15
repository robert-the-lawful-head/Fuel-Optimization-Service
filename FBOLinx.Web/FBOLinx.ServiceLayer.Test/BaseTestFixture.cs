using System;
using FBOLinx.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace FBOLinx.ServiceLayer.Test
{
    [TestFixture]
    public class BaseTestFixture<T>
    {
        protected T subject;

        protected void Arrange(Action<ServiceCollection> arrange)
        {
            var services = new ServiceCollection();
            ServiceConfiguration.Configure(services, GetConfiguration());
            arrange(services);
            var serviceProvider = services.BuildServiceProvider();
            subject = serviceProvider.GetRequiredService<T>();
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