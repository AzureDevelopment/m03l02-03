using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class LogGeneratedExcel
    {
        [FunctionName("LogGeneratedExcel")]
        public static void Run([BlobTrigger("excel/{name}", Connection = "m3l2_STORAGE")] Stream myBlob, [CosmosDB("Test", "Test", ConnectionStringSetting = "db_connection")] out dynamic document, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                myBlob.CopyTo(memoryStream);
                document = new ExcelGenerated()
                {
                    ExcelBase64 = Convert.ToBase64String(memoryStream.ToArray()),
                    Date = DateTime.Now,
                    Name = name
                };
            }
        }
    }
}
