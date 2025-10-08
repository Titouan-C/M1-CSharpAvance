namespace M1_NotedExerciceConversion.Services
{
    /**
     * Service for file operations such as exporting JSON to a file
     */
    public static class FileService
    {
        /**
         * Export a JSON string to a file.
         */
        public static void ExportJsonToFile(string jsonString)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the file: {ex.Message}");
            }
        }
    }
}