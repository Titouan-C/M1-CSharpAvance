// Logiciel de conversion d'un CSV vers un JSON avec LINQ

// Un exemple de CSV est disponible dans "CSV\import_places_salon_provence copy.csv"
// Le CSV est au format :
// "Name";"Longitude";"Latitude";"Description";"Categories"
// "Name1";"Longitude1";"Latitude1";"Description1";"Category1;Category2"

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

using M1_NotedExerciceConversion.Services;

// Initialize services
var converter = new CsvToJsonConverter();
var jsonOperations = new JsonOperations();

// Convert CSV to JSON
string jsonString = converter.ConvertCsvToJson();
if (jsonString == null)
{
    Console.WriteLine("Conversion failed. Exiting...");
    return;
}

// Main program
bool isExiting = false;
while (!isExiting)
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
            string field = Console.ReadLine()?.ToLower();
            Console.WriteLine("Enter the value to search for:");
            string value = Console.ReadLine()?.ToLower();
            
            var results = jsonOperations.SearchInJson(jsonString, field, value);
            if (results == null)
            {
                Console.WriteLine("Search failed or no results found. Keep old JSON data.");
            }
            else
            {
                jsonString = results;
            }
            break;
            
        case "2":
            Console.WriteLine("Enter the field to sort by (e.g., Name, Description):");
            string sortField = Console.ReadLine()?.ToLower();
            Console.WriteLine("Enter the corresponding number to choose the sort order:");
            Console.WriteLine("1. Ascending");
            Console.WriteLine("2. Descending");
            string sortOrderChoice = Console.ReadLine();
            
            var sortedJson = jsonOperations.SortJsonByField(jsonString, sortField, 
                sortOrderChoice == "2" ? "desc" : "asc");
            if (sortedJson == null)
            {
                Console.WriteLine("Sorting failed. Keep old JSON data.");
            }
            else
            {
                jsonString = sortedJson;
            }
            break;
            
        case "3":
            FileService.ExportJsonToFile(jsonString);
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