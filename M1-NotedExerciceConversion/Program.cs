// Logiciel de conversion d'un CSV vers un JSON avec LINQ
// Le CSV est au format :
// "Name";"Longitude";"Latitude";"Description";"Categories"
// "Name1";"Longitude1";"Latitude1";"Description1";"Category1;Category2"
//
// Le JSON est au format :
// [
//   {
//     "Name": "Name1",
//     "Longitude": "Longitude1",
//     "Latitude": "Latitude1",
//     "Description": "Description1",
//     "Categories": ["Category1", "Category2"]
//   },
//   ...
// ]
using System.Text.Json;
using System.Text.RegularExpressions;

string SEPARATOR = ";";

/**
 * Function to clean the Categories field of the CSV file by splitting it into an array.
 */
string[] CleanCategoriesLine(string line)
{
    return line.Split(SEPARATOR).Select(h => h.Trim('"')).ToArray();
}

/**
 * Function to clean a CSV line by extracting values between quotes.
 */
string[] CleanCsvLine(string line)
{
    var matches = Regex.Matches(line, "\"([^\"]*)\"");
    return matches.Cast<Match>().Select(m => m.Groups[1].Value).ToArray();
}

/**
 * Verify if a key exists in a dictionary (case insensitive).
 */
bool HasKeyIgnoreCase(Dictionary<string, object> dict, string key)
{
    return dict.Keys.Any(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase));
}

/**
 * Get a value from a dictionary by key (case insensitive).
 * Return null if the key does not exist.
 */
object GetValueIgnoreCase(Dictionary<string, object> dict, string key)
{
    var foundKey = dict.Keys.FirstOrDefault(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase));
    return foundKey != null ? dict[foundKey] : null;
}

// Logiciel de conversion avec prévisualisation (sans sauvegarde)
// => l'utilisateur fait une recherche et/ou tri
// => on affiche le résultat
// => l'utilisateur peut choisir de sauvegarder le résultat dans un fichier JSON

/**
 * Function to verify the headers of the CSV file.
 * 
 * Return true if headers are valid, false otherwise.
 */
bool VerifyCsvHeaders(string[] headers)
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

/**
 * Function to convert a CSV file to a JSON file.
 * 
 * Return the visualized JSON string or null if an error occurred.
 */
string ConvertCsvToJson()
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
        var headers = CleanCsvLine(lines[0]);
        if (!VerifyCsvHeaders(headers))
        {
            Console.WriteLine("CSV headers are invalid. Exiting...");
            return null;
        }
        var jsonList = new List<Dictionary<string, object>>();
        foreach (var line in lines.Skip(1))
        {
            var values = CleanCsvLine(line);
            if (values.Length != headers.Length)
            {
                Console.WriteLine("Mismatch between header and data columns. Skipping line.");
                continue;
            }
            var jsonObject = new Dictionary<string, object>();
            for (int i = 0; i < headers.Length; i++)
            {
                jsonObject[headers[i]] = headers[i] == "Categories" ? CleanCategoriesLine(values[i]) : values[i];
            }
            jsonList.Add(jsonObject);
        }
        var jsonString = JsonSerializer.Serialize(jsonList, new JsonSerializerOptions {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        Console.WriteLine("Conversion successful. Here is a preview of the JSON data:");
        Console.WriteLine(jsonString);
        return jsonString;
    } catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        return null;
    }
}

/**
 * Function to search into a JSON string by a specific field and value.
 * 
 * Return the JSON results as a string or null if an error occurred.
 */
string SearchInJson(string jsonString, string field, string value)
{
    try
    {
        var jsonList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonString);
        if (jsonList == null)
        {
            Console.WriteLine("JSON file is empty or invalid. Exiting...");
            return null;
        }
        var results = jsonList
            .Where(obj => HasKeyIgnoreCase(obj, field) && GetValueIgnoreCase(obj, field)?.ToString()
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
                Name = obj.ContainsKey("Name") ? obj["Name"] : null,
                Longitude = obj.ContainsKey("Longitude") ? obj["Longitude"] : null,
                Latitude = obj.ContainsKey("Latitude") ? obj["Latitude"] : null,
                Description = obj.ContainsKey("Description") ? obj["Description"] : null,
                Categories = obj.ContainsKey("Categories") ? obj["Categories"] : null
            })
            .ToList();
        var searchResultsJsonString = JsonSerializer.Serialize(resultsJsonString, new JsonSerializerOptions {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        Console.WriteLine("Search results:");
        Console.WriteLine(searchResultsJsonString);
        return searchResultsJsonString;
    } catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        return null;
    }
}

/**
 * Function to sort a JSON string by a specific field.
 * 
 * Return the sorted JSON string or null if an error occurred.
 */
string SortJsonByField(string jsonString, string field, string order)
{
    try
    {
        var jsonList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonString);
        if (jsonList == null)
        {
            Console.WriteLine("JSON file is empty or invalid. Exiting...");
            return null;
        }
        var sortedList = (
            order == "desc"
                ? jsonList.Where(obj => HasKeyIgnoreCase(obj, field))
                    .OrderByDescending(obj => GetValueIgnoreCase(obj, field)?.ToString())
                : jsonList.Where(obj => HasKeyIgnoreCase(obj, field))
                    .OrderBy(obj => GetValueIgnoreCase(obj, field)?.ToString())
            )
            .Select(obj => new
            {
                Name = obj.ContainsKey("Name") ? obj["Name"] : null,
                Longitude = obj.ContainsKey("Longitude") ? obj["Longitude"] : null,
                Latitude = obj.ContainsKey("Latitude") ? obj["Latitude"] : null,
                Description = obj.ContainsKey("Description") ? obj["Description"] : null,
                Categories = obj.ContainsKey("Categories") ? obj["Categories"] : null
            })
            .ToList();
        var sortedListJsonString = JsonSerializer.Serialize(sortedList, new JsonSerializerOptions {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        Console.WriteLine("Sorted JSON data:");
        Console.WriteLine(sortedListJsonString);
        return sortedListJsonString;

    } catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
        return null;
    }
}

/**
 * Function to export a JSON string to a file.
 */
void ExportJsonToFile(string jsonString)
{
    Console.WriteLine("Please enter the path to save the JSON file (e.g., output.json):");
    string jsonFilePath = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(jsonFilePath))
    {
        Console.WriteLine("Invalid file path. Exiting...");
        return;
    }
    try
    {
        File.WriteAllText(jsonFilePath, jsonString);
        Console.WriteLine($"JSON data successfully exported to {jsonFilePath}");
    } catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while saving the file: {ex.Message}");
    }
}

string jsonString = ConvertCsvToJson();
if (jsonString == null)
{
    Console.WriteLine("Conversion failed. Exiting...");
    return;
}

bool isExiting = false;
while (isExiting == false)
{
    Console.WriteLine("Would you like to: (press the corresponding number)");
    Console.WriteLine("1. Search into the converted JSON file");
    Console.WriteLine("2. Sort the JSON file by a specific field");
    Console.WriteLine("3. Export the JSON to a file");
    Console.WriteLine("4. Exit");
    string choice = Console.ReadLine();
    switch (choice)
    {
        case "1":
            Console.WriteLine("Enter the field to search by (e.g., Name, Description):");
            string field = Console.ReadLine().ToLower();
            Console.WriteLine("Enter the value to search for:");
            string value = Console.ReadLine().ToLower();
            var results = SearchInJson(jsonString, field, value);
            if (results == null)
            {
                Console.WriteLine($"Search failed or no results found. Keep old JSON data: {jsonString}");
            } else
            {
                jsonString = results;
            }
            break;
        case "2":
            Console.WriteLine("Enter the field to sort by (e.g., Name, Description):");
            string sortField = Console.ReadLine().ToLower();
            Console.WriteLine($"Enter the corresponding number to choose the sort order:");
            Console.WriteLine("1. Ascending");
            Console.WriteLine("2. Descending");
            string sortOrderChoice = Console.ReadLine();
            var sortedJson = SortJsonByField(jsonString, sortField, sortOrderChoice == "2" ? "desc" : "asc");
            if (sortedJson == null)
            {
                Console.WriteLine($"Sorting failed. Keep old JSON data: {jsonString}");
            } else
            {
                jsonString = sortedJson;
            }
            break;
        case "3":
            ExportJsonToFile(jsonString);
            Console.WriteLine("Exported successfully. Exiting...");
            isExiting = true;
            break;
        case "4":
            Console.WriteLine("Exiting...");
            isExiting = true;
            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}