using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;

namespace Company.Function
{
    public class GetGeneratedExcels
    {
        private readonly CosmosClient _dbClient;

        public GetGeneratedExcels(CosmosClient dbClient)
        {
            _dbClient = dbClient;
        }

        [FunctionName("GetGeneratedExcels")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            List<ExcelGenerated> result = new List<ExcelGenerated>();

            FeedIterator<ExcelGenerated> iterator = _dbClient.GetDatabase("Test").GetContainer("Test").GetItemQueryIterator<ExcelGenerated>("SELECT * FROM c");

            while (iterator.HasMoreResults)
            {
                FeedResponse<ExcelGenerated> currentResultSet = await iterator.ReadNextAsync();
                foreach (ExcelGenerated excel in currentResultSet)
                {
                    result.Add(excel);
                }
            }

            return new OkObjectResult(result);
        }
    }
}
