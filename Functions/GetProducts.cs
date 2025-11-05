using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using yannicktest.Models; 
using System.Net;
using System.Threading.Tasks;

namespace yannicktest.Functions
{
    public class GetProducts
    {
        private readonly ILogger _logger;

        public GetProducts(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetProducts>();
        }

        [Function("GetProducts")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")] HttpRequestData req)
        {
            _logger.LogInformation("Fetching all products...");

            var products = new[]
 {
    new Product { Id = 1, Name = "Product 1", Price = 10.0m },
    new Product { Id = 2, Name = "Product 2", Price = 20.0m },
    new Product { Id = 3, Name = "Product 3", Price = 30.0m }
};


            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(products);
            return response;
        }
    }
}
