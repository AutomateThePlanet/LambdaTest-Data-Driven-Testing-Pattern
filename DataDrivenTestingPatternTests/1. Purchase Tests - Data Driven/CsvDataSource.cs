using System.Globalization;
using System.IO;
using CsvHelper;

namespace DataDrivenTestingPatternTests.FirstVersion;
public class CsvDataSource
{
    public static IEnumerable<TestCaseData> GetDataFromCsv()
    {
        var path = "VatTestData.csv"; // Update with the correct path
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        foreach (var record in csv.GetRecords<dynamic>())
        {
            yield return new TestCaseData(
                Convert.ToInt32(record.Quantity),
                Convert.ToString(record.Country),
                Convert.ToDouble(record.VATRate),
                Convert.ToString(record.ExpectedVAT),
                Convert.ToString(record.ExpectedTotal)
            );
        }
    }
}
