using NUnit.Framework;
using FluentAssertions;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ApiAutomation.Tests
{
    /// <summary>
    /// Contains tests for DELETE operations on API objects.
    /// Verifies behavior for existing, non-existing, and missing ID scenarios.
    /// </summary>
    [TestFixture]
    public class DeleteTests : TestBase
    {
        /// <summary>
        /// Deletes an object that was just created.
        /// Verifies that:
        /// 1. The delete operation succeeds (HTTP 200 OK)
        /// 2. Subsequent GET returns 404 NotFound
        /// </summary>
        [Test]
        public void Delete_ExistingObject_ShouldReturnSuccess()
        {
            // Arrange: Create a new object using test data
            var createPayload = new
            {
                name = testData.createObject.name,
                data = testData.createObject.data
            };
            var createResponse = _endpoint.CreateObject(createPayload);

            // Extract the ID of the newly created object from the JSON response
            var id = JObject.Parse(createResponse.Content)["id"]?.ToString();

            // Act: Delete the object using its ID
            var deleteResponse = _endpoint.DeleteObject(id);

            // Log response for debugging or review
            TestContext.WriteLine("Response Content: " + deleteResponse.Content);
            TestContext.WriteLine("Status Code: " + deleteResponse.StatusCode);

            // Assert: Delete should return HTTP 200 OK
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verify that the object no longer exists
            var getAfter = _endpoint.GetObject(id);
            TestContext.WriteLine("Get after delete: " + getAfter.Content);

            // Assert: GET after delete should return 404 NotFound
            getAfter.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Attempts to delete a non-existing object.
        /// Verifies API behavior is either 200 OK or 404 NotFound.
        /// </summary>
        [Test]
        public void Delete_NonExistingObject_ShouldReturnNotFoundOrSuccess()
        {
            // Arrange: Use a fake ID that does not exist
            var fakeId = "nonexistent-id-123";

            // Act: Attempt to delete non-existing object
            var deleteResponse = _endpoint.DeleteObject(fakeId);

            // Log response
            TestContext.WriteLine("Response Content: " + deleteResponse.Content);
            TestContext.WriteLine("Status Code: " + deleteResponse.StatusCode);

            // Assert: API may either return OK or NotFound
            deleteResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Attempts to delete without providing an ID (hits root endpoint).
        /// Verifies API behavior is handled gracefully.
        /// </summary>
        [Test]
        public void Delete_WithoutId_ShouldReturnRootResponseOrError()
        {
            // Act: Call delete with empty ID (root endpoint)
            var deleteResponse = _endpoint.DeleteObject("");

            // Log response
            TestContext.WriteLine("Response Content: " + deleteResponse.Content);
            TestContext.WriteLine("Status Code: " + deleteResponse.StatusCode);

            // Assert: API may return OK or NotFound, depending on implementation
            deleteResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
        }
    }
}
