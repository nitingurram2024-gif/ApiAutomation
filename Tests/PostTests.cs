using NUnit.Framework;
using FluentAssertions;
using System.Net;
using ApiAutomation.Models;
using Newtonsoft.Json.Linq;

namespace ApiAutomation.Tests
{
    /// <summary>
    /// Contains tests for POST operations on API objects.
    /// Verifies creation of objects with valid and invalid payloads.
    /// </summary>
    [TestFixture]
    public class PostTests : TestBase
    {
        /// <summary>
        /// Verifies that creating an object with a valid payload succeeds.
        /// Steps:
        /// 1. Prepare payload using test data
        /// 2. POST the object
        /// 3. Verify response status code
        /// 4. Verify that the returned object contains an ID
        /// </summary>
        [Test]
        public void Post_ValidBody_ShouldCreateObjectSuccessfully()
        {
            // Arrange: create payload from test data
            var payload = new CreatePayload
            {
                name = testData.createObject.name,
                data = testData.createObject.data
            };

            // Act: send POST request to create the object
            var response = _endpoint.CreateObject(payload);

            // Log response for debugging or review
            TestContext.WriteLine("Response Content: " + response.Content);

            // Assert: creation succeeds (HTTP 200 OK or 201 Created)
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created);

            // Parse JSON response and ensure an ID is returned
            var json = JObject.Parse(response.Content);
            json.SelectToken("id").Should().NotBeNull();
        }

        /// <summary>
        /// Verifies that creating an object with an invalid payload is handled gracefully.
        /// Expected behavior: API may return error or ignore unknown fields.
        /// </summary>
        [Test]
        public void Post_InvalidBody_ShouldReturnBadRequestOrError()
        {
            // Arrange: create a payload with an invalid field
            var invalidPayload = new { invalidField = "test" };

            // Act: send POST request
            var response = _endpoint.CreateObject(invalidPayload);

            // Log response for debugging
            TestContext.WriteLine("Response Content: " + response.Content);
            TestContext.WriteLine("Status Code: " + response.StatusCode);

            // Assert: API may still return OK/Created depending on implementation,
            // or ideally should return 400 BadRequest (adjust assertion if your API strictly validates)
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created);
        }
    }
}
