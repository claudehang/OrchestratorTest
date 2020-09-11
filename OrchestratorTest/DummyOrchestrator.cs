using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace OrchestratorTest
{
    public static class DummyOrchestrator
    {
        [FunctionName("DummyOrchestrator")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            // ver 00
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("DummyOrchestrator_Hello", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>("DummyOrchestrator_Hello", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("DummyOrchestrator_Hello", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;

            // ver 01
            //var outputs = new List<string>();
            //using TestObject testObject = new TestObject();

            //// Replace "hello" with the name of your Durable Activity Function.
            //outputs.Add(await context.CallActivityAsync<string>("DummyOrchestrator_Hello", "Tokyo"));
            //outputs.Add(await context.CallActivityAsync<string>("DummyOrchestrator_Hello", "Seattle"));
            //outputs.Add(await context.CallActivityAsync<string>("DummyOrchestrator_Hello", "London"));

            //// returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            //log.LogError("Orchestrator is going to finish");

            //return outputs;

            // ver 02
            //var outputs = new List<string>();
            //using (TestObject testObject = new TestObject())
            //{
            //    // Replace "hello" with the name of your Durable Activity Function.
            //    outputs.Add(await context.CallActivityAsync<string>("DummyOrchestrator_Hello", "Tokyo"));
            //    outputs.Add(await context.CallActivityAsync<string>("DummyOrchestrator_Hello", "Seattle"));
            //    outputs.Add(await context.CallActivityAsync<string>("DummyOrchestrator_Hello", "London"));

            //    // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            //    log.LogError("Orchestrator is going to finish");
            //}

            //return outputs;
        }

        [FunctionName("DummyOrchestrator_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name} begin");
            using TestObject testObject = new TestObject();
            //using (TestObject testObject = new TestObject())
            //{
            //}
            log.LogInformation($"Saying hello to {name} end");

            return $"Hello {name}!";
        }

        [FunctionName("DummyOrchestrator_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("DummyOrchestrator", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}