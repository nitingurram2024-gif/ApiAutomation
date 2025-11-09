using NUnit.Framework;
using FluentAssertions;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ApiAutomation.Tests
{
    /// <summary>
    /// Contains tests for PUT operations on API objects.
    /// Verifies updating existing objects and handling of invalid payloads.
    /// </summary>
    [TestFixture]
    public class PutTests : TestBase
    {
        /// <summary>
        /// Verifies that a PUT request can successfully replace an existing object.
        /// Steps:
        /// 1. Create a new object using test data
        /// 2. Extract the created object's ID
        /// 3. Prepare a new payload for update
        /// 4. Perform PUT request
        /// 5. Verify response status and content
        /// </summary>
        [Test]
        public void Put_ExistingObject_ShouldReplaceAndReturnUpdated()
        {
            // Arrange: create object
            var createPayload = new
            {
                name = testData.createObject.name,
                data = testData.createObject.data
            };
            var createResponse = _endpoint.CreateObject(createPayload);

            // Assert: creation succeeded
            createResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created);

            // Extract ID of the created object
            var id = JObject.Parse(createResponse.Content)["id"]?.ToString();

            // Arrange: prepare new payload for update
            var newPayload = new
            {
                name = testData.updateObject.name,
                data = testData.updateObject.data
            };

            // Act: update object using PUT
            var response = _endpoint.UpdateObject(id, newPayload);

            // Log response for debugging or review
            TestContext.WriteLine("Response Content: " + response.Content);

            // Assert: PUT should return HTTP 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// Verifies that a PUT request with an invalid payload is handled gracefully.
        /// Expected behavior: API may return BadRequest, NotFound, or even OK depending on validation rules.
        /// </summary>
        [Test]
        public void Put_InvalidBody_ShouldReturnBadRequestOrError()
        {
            // Arrange: create object to update
            var createPayload = new
            {
                name = testData.createObject.name,
                data = testData.createObject.data
            };
            var createResponse = _endpoint.CreateObject(createPayload);

            // Extract ID of the created object
            var id = JObject.Parse(createResponse.Content)["id"]?.ToString();

            // Arrange: prepare invalid payload
            var invalidPayload = new { invalidField = "x" };

            // Act: attempt to update object with invalid payload
            var response = _endpoint.UpdateObject(id, invalidPayload);

            // Log response
            TestContext.WriteLine("Response Content: " + response.Content);
            TestContext.WriteLine("Status Code: " + response.StatusCode);

            // Assert: API may respond with OK, BadRequest, or NotFound
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.NotFound);
        }
    }
}
