using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Notes.Tests
{
    public class NotesControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public NotesControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostAddsNewNote()
        {
            // Arrange
            var newNoteJson = "{ \"Title\": \"title X\", \"Content\": \"content X\" }";
            var httpContent = new StringContent(newNoteJson, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/api/notes", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsBadRequestForMissingContent()
        {
            // Arrange
            var newNoteJson = "{ \"Title\": \"title X\" }";
            var httpContent = new StringContent(newNoteJson, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/api/notes", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsBadRequestForMissingTitle()
        {
            // Arrange
            var newNoteJson = "{ \"Content\": \"content X\" }";
            var httpContent = new StringContent(newNoteJson, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await _client.PostAsync("/api/notes", httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
