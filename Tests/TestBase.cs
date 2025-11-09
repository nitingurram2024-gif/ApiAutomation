using NUnit.Framework;
using Newtonsoft.Json;
using System.IO;
using System;
using ApiAutomation.Endpoints;
using ApiAutomation.Models;

namespace ApiAutomation.Tests
{
    /// <summary>
    /// Base class for all API tests.
    /// Handles:
    /// 1. Initialization of endpoint objects
    /// 2. Loading test data from JSON
    /// 3. Provides shared properties accessible in all derived test classes
    /// </summary>
    public class TestBase
    {
        /// <summary>
        /// Encapsulates API endpoint methods for Object CRUD operations.
        /// </summary>
        protected ObjectEndpoint _endpoint;

        /// <summary>
        /// Holds deserialized test data from Data/testdata.json
        /// </summary>
        protected TestData testData;

        /// <summary>
        /// One-time setup that runs before any tests in the derived classes.
        /// Initializes endpoint and loads test data.
        /// </summary>
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            // Initialize the endpoint object (POM-style API wrapper)
            _endpoint = new ObjectEndpoint();

            // Determine the path to testdata.json
            var dataPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Data", "testdata.json");

            // Fallback to AppDomain path if not found in TestDirectory (useful for VS vs CI)
            if (!File.Exists(dataPath))
            {
                dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "testdata.json");
            }

            // Throw an exception if test data file is still not found
            if (!File.Exists(dataPath))
            {
                throw new FileNotFoundException("testdata.json not found at: " + dataPath);
            }

            // Read JSON file content
            var json = File.ReadAllText(dataPath);

            // Deserialize JSON into TestData object for easy access in tests
            testData = JsonConvert.DeserializeObject<TestData>(json);

            // Log the loaded test data path for review/debugging
            TestContext.WriteLine("Test data loaded from: " + dataPath);
        }
    }
}
