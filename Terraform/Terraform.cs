using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using System.Text.Json;
using Terraform.Providers;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Terraform
{
	public class Terraform
	{
		public bool DryRun { get;set;}
		public DirectoryInfo Path { get; set; }
		public Dictionary<string, object> Vars { get; set; }
		public List<Provider> Providers { get; set; }

		public Terraform()
		{
			this.Path = new DirectoryInfo(Environment.CurrentDirectory);
			this.Vars = new Dictionary<string, object>();
			this.Providers = new List<Provider>();
		}
		public async Task<string> Init(ILogger log)
		{
			return await command("init", log);
		}

		public async Task<string> Validate(ILogger log)
		{
			return await command("validate", log);
		}

		public async Task<string> Plan(ILogger log)
		{
			return await command("plan", log, true);
		}

		public async Task<string> Apply(ILogger log)
		{
			return await command("apply", log, true);
		}

		public async Task<string> Destroy(ILogger log)
		{
			return await command("destroy", log);
		}

		private async Task<string> command(string action, ILogger log, bool withVars = false)
		{
			var command = Cli.Wrap("terraform")
				.WithWorkingDirectory(this.Path.FullName);

			var arguments = new List<string>();
			arguments.Add(action);
			if (withVars)
			{
				foreach (KeyValuePair<string, object> var in this.Vars)
				{
					var value = var.Value;
					if(value.GetType() != typeof(string))
						value = JsonSerializer.Serialize(var.Value);
					arguments.Add($"-var='{var.Key}={value}'");
				}
			}
			command = command.WithArguments(arguments, false);
			command = command.WithEnvironmentVariables(this.combinedProviderConfigs);

			log.LogInformation($"Executing: {command}");
			if(DryRun){
				return command.ToString();
			}
			var task = await command.ExecuteBufferedAsync();
			log.LogInformation($"{task.StandardOutput}");
			return task.StandardOutput;
		}

		private Dictionary<string, string> combinedProviderConfigs
		{
			get
			{
				return this.Providers.Select( p => p.GetConfig())
					.SelectMany(c => c).ToDictionary(pair => pair.Key, pair => pair.Value);
			}
		}
	}
}