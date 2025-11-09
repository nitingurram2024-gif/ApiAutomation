using NUnit.Framework;
using FluentAssertions;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ApiAutomation.Tests
{
    /// <summary>
    /// Contains tests for GET operations on API objects.
    /// Verifies retrieval of existing objects and proper handling of invalid or non-existing IDs.
    /// </summary>
    [TestFixture]
    public class GetTests : TestBase
    {
        /// <summary>
        /// Verifies that an existing object can be retrieved successfully.
        /// Steps:
        /// 1. Create a new object using test data
        /// 2. Extract the returned ID
        /// 3. Perform GET request with the ID
        /// 4. Verify response status and content
        /// </summary>
        [Test]
        public void Get_ExistingObject_ShouldReturnObject()
        {
            // Arrange: create object payload from test data
            var createPayload = new
            {
                name = testData.createObject.name,
                data = testData.createObject.data
            };

            // Act: create the object
            var createResponse = _endpoint.CreateObject(createPayload);

            // Assert: creation succeeds (either 200 OK or 201 Created)
            createResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created);

            // Extract the ID of the created object
            var createdId = JObject.Parse(createResponse.Content)["id"]?.ToString();

            // Act: GET the object by ID
            var response = _endpoint.GetObject(createdId);

            // Log response for debugging or review
            TestContext.WriteLine("Response Content: " + response.Content);

            // Assert: GET should return 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verify that the returned object's ID matches the created ID
            var json = JObject.Parse(response.Content);
            json.SelectToken("id").ToString().Should().Be(createdId);
        }

        /// <summary>
        /// Verifies that GET on a non-existing ID returns 404 NotFound.
        /// </summary>
        [Test]
        public void Get_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange: use a fake/non-existing ID
            var response = _endpoint.GetObject("non-existing-id-12345");

            // Log response
            TestContext.WriteLine("Response Content: " + response.Content);

            // Assert: API returns 404 NotFound
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Verifies that GET with an invalid ID format is handled gracefully.
        /// Expected behavior: either 400 BadRequest or 404 NotFound.
        /// </summary>
        [Test]
        public void Get_InvalidIdFormat_ShouldReturnBadRequestOrNotFound()
        {
            // Arrange: use an invalid ID format
            var response = _endpoint.GetObject("!@#$%^&*()");

            // Log response
            TestContext.WriteLine("Response Content: " + response.Content);

            // Assert: status code is either 400 or 404
            ((int)response.StatusCode).Should().BeOneOf(
                (int)HttpStatusCode.BadRequest,
                (int)HttpStatusCode.NotFound
            );
        }
    }
}
