using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using yannicktest.Models;

namespace yannicktest.Functions
{
    public class GetCommand
    {
        private readonly ILogger _logger;

        public GetCommand(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetCommand>();
        }

        [Function("GetCommand")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "commands")] HttpRequestData req)
        {
            _logger.LogInformation("Fetching all commands...");

            var commands = new[]
            {
                new Command { Id = 1, Description = "Command 1", Amount = 100.0m },
                new Command { Id = 2, Description = "Command 2", Amount = 200.0m },
                new Command { Id = 3, Description = "Command 3", Amount = 300.0m }
            };

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(commands);
            return response;
        }


    }
}