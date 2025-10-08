using System.Text.Json;
using M1_NotedExerciceConversion.Helpers;

namespace M1_NotedExerciceConversion.Services
{
    /**
     * Service for JSON operations such as searching and sorting
     */
    public class JsonOperations
    {
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonOperations()
        {
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        /**
         * Search for entries in a JSON string by a specific field and value.
         * 
         * Args:
         * - jsonString: JSON string to search
         * - field: Field to search by (e.g., "Name", "Description")
         * - value: Value to search for
         * 
         * Returns: search results as a JSON string or null if no results found or an error occurred
         */
        public string SearchInJson(string jsonString, string field, string value)
        {
            try
            {
                var jsonList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonString);
                if (jsonList == null)
                {
                    Console.WriteLine("JSON file is empty or invalid. Exiting...");
                    return null;
                }

                // Search with LINQ
                var results = jsonList
                    .Where(obj => DictionaryHelper.HasKeyIgnoreCase(obj, field) && 
                        DictionaryHelper.GetValueIgnoreCase(obj, field)?.ToString()
                        ?.Contains(value, StringComparison.OrdinalIgnoreCase) == true)
                    .ToList();

                if (results.Count == 0)
                {
                    Console.WriteLine("No matching entries found.");
                    return null;
                }

                var resultsJsonString = results
                    .Select(obj => new
                    {
                        Name = DictionaryHelper.GetValueIgnoreCase(obj, "Name"),
                        Longitude = DictionaryHelper.GetValueIgnoreCase(obj, "Longitude"),
                        Latitude = DictionaryHelper.GetValueIgnoreCase(obj, "Latitude"),
                        Description = DictionaryHelper.GetValueIgnoreCase(obj, "Description"),
                        Categories = DictionaryHelper.GetValueIgnoreCase(obj, "Categories")
                    })
                    .ToList();

                var searchResultsJsonString = JsonSerializer.Serialize(resultsJsonString, _jsonOptions);
                Console.WriteLine("Search results:");
                Console.WriteLine(searchResultsJsonString);
                return searchResultsJsonString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        /**
         * Sort a JSON string by a specific field in ascending or descending order.
         * 
         * Args:
         * - jsonString: JSON string to sort
         * - field: Field to sort by (e.g., "Name", "Description")
         * - order: "asc" for ascending, "desc" for descending
         * 
         * Returns: sorted JSON string or null if an error occurred
         */
        public string SortJsonByField(string jsonString, string field, string order)
        {
            try
            {
                var jsonList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonString);
                if (jsonList == null)
                {
                    Console.WriteLine("JSON file is empty or invalid. Exiting...");
                    return null;
                }

                // Sort with LINQ
                var sortedList = (order == "desc"
                        ? jsonList.Where(obj => DictionaryHelper.HasKeyIgnoreCase(obj, field))
                            .OrderByDescending(obj => DictionaryHelper.GetValueIgnoreCase(obj, field)?.ToString())
                        : jsonList.Where(obj => DictionaryHelper.HasKeyIgnoreCase(obj, field))
                            .OrderBy(obj => DictionaryHelper.GetValueIgnoreCase(obj, field)?.ToString())
                    )
                    .Select(obj => new
                    {
                        Name = DictionaryHelper.GetValueIgnoreCase(obj, "Name"),
                        Longitude = DictionaryHelper.GetValueIgnoreCase(obj, "Longitude"),
                        Latitude = DictionaryHelper.GetValueIgnoreCase(obj, "Latitude"),
                        Description = DictionaryHelper.GetValueIgnoreCase(obj, "Description"),
                        Categories = DictionaryHelper.GetValueIgnoreCase(obj, "Categories")
                    })
                    .ToList();

                var sortedListJsonString = JsonSerializer.Serialize(sortedList, _jsonOptions);
                Console.WriteLine("Sorted JSON data:");
                Console.WriteLine(sortedListJsonString);
                return sortedListJsonString;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }
}