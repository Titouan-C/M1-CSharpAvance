namespace M1_NotedExerciceConversion.Helpers
{
    /**
     * Helper class for dictionary operations
     */
    public static class DictionaryHelper
    {
        /**
         * Check if a dictionary contains a key (case insensitive).
         * 
         * Args:
         * - dict: The dictionary to check
         * - key: The key to look for
         * 
         * Returns: True if the key exists, false otherwise
         */
        public static bool HasKeyIgnoreCase(Dictionary<string, object> dict, string key)
        {
            return dict.Keys.Any(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase));
        }

        /**
         * Get the value associated with a key in a dictionary (case insensitive).
         * 
         * Args:
         * - dict: The dictionary to search
         * - key: The key to look for
         * 
         * Returns: The value if found, null otherwise
         */
        public static object GetValueIgnoreCase(Dictionary<string, object> dict, string key)
        {
            var foundKey = dict.Keys.FirstOrDefault(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase));
            return foundKey != null ? dict[foundKey] : null;
        }
    }
}