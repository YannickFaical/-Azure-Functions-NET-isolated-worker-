using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using yannicktest.Models;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace testyannick.Functions
{
    public class AddCommand
    {
        private readonly ILogger _logger;

        public AddCommand(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AddCommand>();
        }

        [Function("AddCommand")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post" ,Route ="add/command")] HttpRequestData req)

        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();
                var command = JsonSerializer.Deserialize<Command>(body);

                if (command == null)
                {
                    var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badResponse.WriteStringAsync("Invalid command data.");
                    return badResponse;
                }

                _logger.LogInformation($"Received command: {command.Description}");
                var response = req.CreateResponse(HttpStatusCode.Created);
                await response.WriteAsJsonAsync(new { message = "Command added successfully", command });
                return response;


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing request: {ex.Message}");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("An error occurred while processing the request.");
                return errorResponse;

            }
            
        }


    }
}