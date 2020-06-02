using System.IO;
using OfficeOpenXml;

public class CsvToExcel
{
    public Stream Convert(string csv)
    {
        Stream output = new MemoryStream();

        using (ExcelPackage excelPackage = new ExcelPackage())
        {
            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Arkusz 1");

            worksheet.Cells["A1"].LoadFromText(csv);
            excelPackage.SaveAs(output);
            output.Position = 0;
        }

        return output;
    }
}