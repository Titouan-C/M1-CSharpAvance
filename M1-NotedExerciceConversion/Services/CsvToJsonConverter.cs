using System.Text.Json;
using M1_NotedExerciceConversion.Services;
using M1_NotedExerciceConversion.Helpers;

namespace M1_NotedExerciceConversion.Services
{
    /**
     * Service to convert CSV files to JSON format.
     */
    public class CsvToJsonConverter
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public CsvToJsonConverter()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        /**
         * Convert a CSV file to JSON format.
         * 
         * Returns: The JSON string if successful, null otherwise.
         */
        public string ConvertCsvToJson()
        {
            Console.WriteLine("--- CSV to JSON Converter ---");
            Console.WriteLine("Please enter the path to the CSV file:");
            string csvFilePath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(csvFilePath) || !File.Exists(csvFilePath))
            {
                Console.WriteLine("Invalid file path. Exiting...");
                return null;
            }

            try
            {
                var lines = File.ReadAllLines(csvFilePath);
                if (lines.Length < 2)
                {
                    Console.WriteLine("CSV file is empty or has no data. Exiting...");
                    return null;
                }

                var headers = CsvParser.CleanCsvLine(lines[0]);
                if (!CsvParser.VerifyCsvHeaders(headers))
                {
                    Console.WriteLine("CSV headers are invalid. Exiting...");
                    return null;
                }

                var jsonList = new List<Dictionary<string, object>>();
                foreach (var line in lines.Skip(1))
                {
                    var values = CsvParser.CleanCsvLine(line);
                    if (values.Length != headers.Length)
                    {
                        Console.WriteLine("Mismatch between header and data columns. Skipping line.");
                        continue;
                    }

                    var jsonObject = new Dictionary<string, object>();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        jsonObject[headers[i]] = headers[i] == "Categories" 
                            ? CsvParser.CleanCategoriesLine(values[i]) 
                            : values[i];
                    }
                    jsonList.Add(jsonObject);
                }

                var jsonString = JsonSerializer.Serialize(jsonList, _jsonOptions);
                Console.WriteLine("Conversion successful. Here is a preview of the JSON data:");
                Console.WriteLine(jsonString);
                return jsonString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}