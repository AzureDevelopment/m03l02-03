using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Company.Function
{
    public class OnCsvCreated
    {
        private readonly CsvToExcel _csvToExcel;

        public OnCsvCreated(CsvToExcel csvToExcel)
        {
            _csvToExcel = csvToExcel;
        }

        [FunctionName("OnCsvCreated")]
        public void Run([BlobTrigger("csv/{name}", Connection = "m3l2_STORAGE")] Stream myBlob, [Blob("excel/output.xlsx", FileAccess.Write)] Stream output, string name, ILogger log)
        {
            TelemetryClient appInsightsClient = new TelemetryClient(new TelemetryConfiguration("1cf361b3-7a90-4416-9088-36b811b35a29"));
            appInsightsClient.TrackEvent("appEvent", new Dictionary<string, string>
            {
                ["progress"] = "Entered OnCsvCreated"
            });

            if (!name.Contains(".csv"))
            {
                throw new Exception("Not a csv file");
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            using (StreamReader streamReader = new StreamReader(myBlob))
            {
                appInsightsClient.TrackEvent("appEvent", new Dictionary<string, string>
                {
                    ["progress"] = "Started reading CSV"
                });
                string csv = streamReader.ReadToEnd();
                _csvToExcel
                    .Convert(csv)
                    .CopyTo(output); appInsightsClient.TrackEvent("appEvent", new Dictionary<string, string>
                    {
                        ["progress"] = "Generated Excel"
                    });
            }
        }
    }
}
