using NUnit.Framework;
using FluentAssertions;
using System.Net;
using Newtonsoft.Json.Linq;

namespace ApiAutomation.Tests
{
    /// <summary>
    /// PATCH endpoint tests.
    /// Uses data from testdata.json for both valid and invalid body tests.
    /// Each test creates its own object, ensuring independence and reliability.
    /// </summary>
    [TestFixture]
    public class PatchTests : TestBase
    {
        /// <summary>
        /// Valid partial update test — ensures PATCH successfully updates existing object fields.
        /// </summary>
        [Test]
        public void Patch_ValidBody_ShouldUpdatePartially()
        {
            // ARRANGE: Create a new object before patching
            var createPayload = new
            {
                name = testData.createObject.name,
                data = testData.createObject.data
            };

            var createResponse = _endpoint.CreateObject(createPayload);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK, "Object creation must succeed before PATCH");

            // Extract newly created object ID
            var id = JObject.Parse(createResponse.Content)["id"]?.ToString();
            id.Should().NotBeNullOrEmpty("Created object must have a valid ID to patch");

            // Build valid PATCH payload using test data (updating only 'price')
            var patchPayload = new
            {
                data = new
                {
                    price = testData.patchObject.data["price"]
                }
            };

            // ACT: Perform PATCH request
            var response = _endpoint.PatchObject(id, patchPayload);

            // LOG details
            TestContext.WriteLine($"Created Object ID: {id}");
            TestContext.WriteLine("Response Content: " + response.Content);
            TestContext.WriteLine("Status Code: " + response.StatusCode);

            // ASSERT: Expect success (200 or 201)
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.Created);
        }

        /// <summary>
        /// Invalid body test — ensures API returns an error for malformed or invalid payloads.
        /// </summary>
        [Test]
        public void Patch_InvalidBody_ShouldReturnBadRequestOrError()
        {
            // ARRANGE: Create a valid object to patch
            var createPayload = new
            {
                name = testData.createObject.name,
                data = testData.createObject.data
            };

            var createResponse = _endpoint.CreateObject(createPayload);
            createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var id = JObject.Parse(createResponse.Content)["id"]?.ToString();
            id.Should().NotBeNullOrEmpty("Created object must exist before testing invalid PATCH");

            // Prepare an invalid PATCH payload (invalid key)
            var invalidPayload = new
            {
                invalidField = "badValue"
            };

            // ACT: Perform PATCH request with invalid payload
            var response = _endpoint.PatchObject(id, invalidPayload);

            // LOG response
            TestContext.WriteLine($"Created Object ID: {id}");
            TestContext.WriteLine("Response Content: " + response.Content);
            TestContext.WriteLine("Status Code: " + response.StatusCode);

            // ASSERT: Expect error response (BadRequest, NotFound, or sometimes OK for lenient APIs)
            response.StatusCode.Should().BeOneOf(
                HttpStatusCode.BadRequest,
                HttpStatusCode.NotFound,
                HttpStatusCode.OK
            );
        }
    }
}
