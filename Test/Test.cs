using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using TF.Providers;
using Xunit;

namespace TF.Test
{
	public class UnitTests
	{
		[Fact]
		public static async void TestConstruction()
		{
			var provider = new Azure()
			{
				ClientId = "abc",
				ClientSecret = "123",
				TenantId = "",
				SubscriptionId = ""
			};

			ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());
			ILogger logger = loggerFactory.CreateLogger<UnitTests>();

			var terraform = new Terraform()
			{
				Path = new DirectoryInfo(Path.GetTempPath()),
				DryRun = true
			};

			terraform.Providers.Add(provider);
			terraform.Vars.Add("adam2", new List<string>() { "how", "many", "is", "this" });
			var configValues = provider.GetConfig();
			var result = await terraform.Plan(logger);

			Assert.Equal("terraform plan -var='adam2=[\"how\",\"many\",\"is\",\"this\"]'", result, ignoreWhiteSpaceDifferences: true);
		}
	}
}