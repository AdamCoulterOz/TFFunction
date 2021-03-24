using System;
using Microsoft.Extensions.Logging;
using TF;
using TF.Providers;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            var provider = new Azure()
			{
				ClientId = "xxxx",
				ClientSecret = "xxxx",
				TenantId = "xxxx",
				SubscriptionId = "xxxx"
			};

			ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
			ILogger logger = loggerFactory.CreateLogger<Program>();

			var tf = new Terraform();

			tf.Providers.Add(provider);
			var configValues = provider.GetConfig();
            logger.LogInformation(tf.Init(logger).Result);
            logger.LogInformation(tf.Validate(logger).Result);
            logger.LogInformation(tf.Plan(logger).Result);
            logger.LogInformation(tf.Apply(logger).Result);
            logger.LogInformation(tf.Destroy(logger).Result);
        }
    }
}
