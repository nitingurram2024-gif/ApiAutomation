using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ApiAutomation.Config
{
    /// <summary>
    /// A static class to read configuration values from appsettings.json
    /// or optionally from environment variables.
    /// </summary>
    public static class ConfigReader
    {
        // Holds the parsed JSON configuration
        private static JObject _config;

        /// <summary>
        /// Static constructor runs once when the class is first accessed.
        /// Reads the appsettings.json file into a JObject.
        /// </summary>
        static ConfigReader()
        {
            try
            {
                // Build the full path to appsettings.json based on the base directory
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "appsettings.json");

                if (File.Exists(path))
                {
                    // If the file exists, read its contents and parse as JSON
                    _config = JObject.Parse(File.ReadAllText(path));
                }
                else
                {
                    // If file does not exist, initialize an empty JObject
                    _config = new JObject();
                }
            }
            catch
            {
                // In case of any exception (file read/parse), fallback to empty JObject
                _config = new JObject();
            }
        }

        /// <summary>
        /// Get a configuration value by key.
        /// Priority: Environment variable (if specified) > JSON config
        /// </summary>
        /// <param name="key">The JSON key (supports nested keys via dot notation, e.g., "ApiSettings.BaseUrl")</param>
        /// <param name="envVarName">Optional environment variable name to override the JSON value</param>
        /// <returns>Configuration value as string, or null if not found</returns>
        public static string Get(string key, string envVarName = null)
        {
            // Check environment variable first if envVarName is provided
            if (!string.IsNullOrEmpty(envVarName))
            {
                var env = Environment.GetEnvironmentVariable(envVarName);
                if (!string.IsNullOrEmpty(env))
                    return env; // Return env var value if set
            }

            // Look up the key in the parsed JSON configuration
            var token = _config.SelectToken(key);

            // Return the value as string, or null if key does not exist
            return token?.ToString();
        }
    }
}
