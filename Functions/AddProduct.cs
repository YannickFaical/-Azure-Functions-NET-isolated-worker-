using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using yannicktest.Models;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;


namespace yannicktest.Functions
{
    public class AddProduct
    {
        private readonly ILogger _logger;

        public AddProduct(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AddProduct>();
        }

        [Function("AddProduct")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")] HttpRequestData req)
        {
            try
            {
                var body = await new StreamReader(req.Body).ReadToEndAsync();
                var product = JsonSerializer.Deserialize<Product>(body);

                if (product == null)
                {
                    var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await badResponse.WriteStringAsync("Invalid product data.");
                    return badResponse;
                }

                _logger.LogInformation($"Product added: {product.Name} - {product.Price} MAD");

                var response = req.CreateResponse(HttpStatusCode.Created);
                await response.WriteAsJsonAsync(new { message = "Product added successfully!", product });
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product");
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("An error occurred.");
                return errorResponse;
            }
        }
    }
}
