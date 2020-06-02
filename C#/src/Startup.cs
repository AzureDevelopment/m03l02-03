using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Company.Function.Startup))]

namespace Company.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<CsvToExcel>();
            builder.Services.AddSingleton<CosmosClient>(new CosmosClient("AccountEndpoint=https://tesst.documents.azure.com:443/;AccountKey=JKD2ILA2Yi0Zd2E84MqlXb2rc5qw30U0whMVXGELNErDBJ2FT8XGPhe1D5f36vM1MbOsXiWiJUUzRx4jaI1mpw==;"));
        }
    }
}