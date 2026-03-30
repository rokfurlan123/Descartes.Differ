using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Descartes.Test.Integration
{
    public class DiffEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public DiffEndpointsTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task SampleFlow_Equals_WhenBothSidesMatch()
        {
            await ArrangeEqualPayloadAsync();
        }

        [Fact]
        public async Task SampleFlow_ContentDoNotMatch_WhenRightChanges()
        {
            var id = await ArrangeEqualPayloadAsync();

            await PutAsync(id, "right", "AQABAQ==", HttpStatusCode.Created);

            var response = await _client.GetAsync($"/v1/diff/{id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal("ContentDoNotMatch", doc.RootElement.GetProperty("diffResultType").GetString());
            var diffs = doc.RootElement.GetProperty("diffs");
            Assert.Equal(2, diffs.GetArrayLength());
            Assert.Equal(0, diffs[0].GetProperty("offset").GetInt32());
            Assert.Equal(1, diffs[0].GetProperty("length").GetInt32());
            Assert.Equal(2, diffs[1].GetProperty("offset").GetInt32());
            Assert.Equal(2, diffs[1].GetProperty("length").GetInt32());
        }

        [Fact]
        public async Task SampleFlow_SizeDoNotMatch_WhenLeftChangesSize()
        {
            var id = await ArrangeEqualPayloadAsync();

            await PutAsync(id, "left", "AAA=", HttpStatusCode.Created);

            var response = await _client.GetAsync($"/v1/diff/{id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            await AssertDiffResultAsync(response, "SizeDoNotMatch");
        }

        [Fact]
        public async Task PutLeft_ReturnsBadRequest_WhenDataNull()
        {
            var id = NewId();

            var response = await _client.PutAsJsonAsync($"/v1/diff/{id}/left", new { data = (string?)null });
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private async Task<string> ArrangeEqualPayloadAsync()
        {
            var id = NewId();

            await AssertStatusAsync($"/v1/diff/{id}", HttpStatusCode.NotFound);
            await PutAsync(id, "left", "AAAAAA==", HttpStatusCode.Created);
            await AssertStatusAsync($"/v1/diff/{id}", HttpStatusCode.NotFound);
            await PutAsync(id, "right", "AAAAAA==", HttpStatusCode.Created);

            var response = await _client.GetAsync($"/v1/diff/{id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            await AssertDiffResultAsync(response, "Equals");

            return id;
        }

        private async Task PutAsync(string id, string side, string base64, HttpStatusCode expected)
        {
            var response = await _client.PutAsJsonAsync($"/v1/diff/{id}/{side}", new { data = base64 });
            Assert.Equal(expected, response.StatusCode);
        }

        private async Task AssertStatusAsync(string url, HttpStatusCode expected)
        {
            var response = await _client.GetAsync(url);
            Assert.Equal(expected, response.StatusCode);
        }

        private static async Task AssertDiffResultAsync(HttpResponseMessage response, string expected)
        {
            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal(expected, doc.RootElement.GetProperty("diffResultType").GetString());
        }

        private static string NewId() => Guid.NewGuid().ToString("N");
    }
}
