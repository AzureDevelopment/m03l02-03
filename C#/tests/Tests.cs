using System.Collections.Generic;
using System.IO;
using System;
using Xunit;
using OfficeOpenXml;
using Company.Function;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSubstitute;

namespace tests
{
    public class Tests
    {
        [Fact]
        public void ShouldGenerateExcel_GivenCsv()
        {
            //Given
            CsvToExcel converter = new CsvToExcel();
            string csv = @"kolumna1,kolumna2
Value1,Value2";

            //When
            Stream result = converter.Convert(csv);

            //Then
            using (ExcelPackage excelPackage = new ExcelPackage(result))
            {
                IEnumerator<ExcelWorksheet> enumerator = excelPackage.Workbook.Worksheets.GetEnumerator();
                enumerator.MoveNext();

                Assert.Equal("kolumna1", enumerator.Current.Cells["A1"].Value);
            }
        }

        [Fact]
        public void ShouldFinishExecutionWithoutErrors()
        {
            //Given
            Startup startup = new Startup();
            IHost host = new HostBuilder()
                .ConfigureWebJobs(startup.Configure)
                .Build();
            OnCsvCreated function = new OnCsvCreated(host.Services.GetService<CsvToExcel>());
            Stream input = GenerateStreamFromString(@"kolumna1,kolumna2
wartosc1,wartosc2");
            Stream output = new MemoryStream();
            ILogger mockedLogger = Substitute.For<ILogger>();

            using (input)
            using (output)
            {
                //When
                function.Run(input, output, "test.csv", mockedLogger);

                //Then
                Assert.True(output.Length > 0);
            }
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
