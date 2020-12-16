using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using CliWrap;
using CliWrap.Buffered;

namespace Terraform
{
    public static class Orchestrator
    {
        [FunctionName("Orchestrator")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();
            outputs.Add(await context.CallActivityAsync<string>("Terraform", "Run the terrform..."));
            return outputs;
        }

        [FunctionName("Terraform")]
        public static async Task<string> Terraform([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Starting Terraform Activity with argument {name}.");
            var task = await Cli.Wrap("terraform")
                .WithWorkingDirectory("/Users/adam/Code/GitHub/AdamCoulterOz/TFFunction/terraform/")
                .WithArguments("init")
                .ExecuteBufferedAsync();

            return task.StandardOutput;
        }

        [FunctionName("Handler")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("Orchestrator", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}