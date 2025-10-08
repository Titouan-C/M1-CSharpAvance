using System.Text.RegularExpressions;

namespace M1_NotedExerciceConversion.Services
{
    /**
     * Class for parsing
     */
    public static class CsvParser
    {
        private const string SEPARATOR = ";";

        /**
         * Clean a categories line by splitting and trimming quotes
         */
        public static string[] CleanCategoriesLine(string line)
        {
            return line.Split(SEPARATOR).Select(h => h.Trim('"')).ToArray();
        }

        /**
         * Clean a CSV line by extracting quoted values
         */
        public static string[] CleanCsvLine(string line)
        {
            var matches = Regex.Matches(line, "\"([^\"]*)\"");
            return matches.Cast<Match>().Select(m => m.Groups[1].Value).ToArray();
        }

        /**
         * Verify that CSV headers match expected format
         */
        public static bool VerifyCsvHeaders(string[] headers)
        {
            var expectedHeaders = new[] { "Name", "Longitude", "Latitude", "Description", "Categories" };
            if (headers.Length != expectedHeaders.Length)
            {
                Console.WriteLine("CSV file does not have the correct number of headers.");
                return false;
            }
            for (int i = 0; i < headers.Length; i++)
            {
                if (!string.Equals(headers[i], expectedHeaders[i], StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Header mismatch: expected '{expectedHeaders[i]}', found '{headers[i]}'");
                    return false;
                }
            }
            return true;
        }
    }
}