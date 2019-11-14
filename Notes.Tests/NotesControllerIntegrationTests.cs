using Microsoft.Extensions.DependencyInjection;
using Notes.Data;
using Notes.Dtos;
using Notes.Models;
using Notes.Tests.Helpers;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Notes.Tests
{
    public class NotesControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly NotesDbContext _dbContext;


        public NotesControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _dbContext = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<NotesDbContext>();

        }

        [Fact]
        public async Task Add_AddsToDbAndReturnsOk_ForProperNote()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newNote = new NoteForAddingDto() { Title = "title X", Content = "content X" };
            var newNoteJson = JsonSerializer.Serialize(newNote);
            var httpContent = new StringContent(newNoteJson, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await client.PostAsync(Consts.postUrl, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(_dbContext.Set<Note>().Find(Consts.newNoteId) != null);
        }

        [Fact]
        public async Task Add_ReturnsBadRequestAndProperContentTypeAndDoesntAddToDb_ForMissingContent()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newNote = new NoteForAddingDto() { Title = "title X" };
            var newNoteJson = JsonSerializer.Serialize(newNote);
            var httpContent = new StringContent(newNoteJson, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await client.PostAsync(Consts.postUrl, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.True(_dbContext.Set<Note>().Find(Consts.newNoteId) == null);
        }

        [Fact]
        public async Task Add_ReturnsBadRequestAndProperContentTypeAndDoesntAddToDb_ForMissingTitle()
        {
            // Arrange
            var client = _factory.CreateClient();
            var newNote = new NoteForAddingDto() { Content = "content" };
            var newNoteJson = JsonSerializer.Serialize(newNote);
            var httpContent = new StringContent(newNoteJson, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await client.PostAsync(Consts.postUrl, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("application/problem+json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.True(_dbContext.Set<Note>().Find(Consts.newNoteId) == null);
        }

        [Fact]
        public async Task GetLatestById_ReturnsOkAndProperContentType_ForProperId()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(Consts.properIdUrl);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetLatestById_ReturnsBadRequestAndProperContentType_ForNonexistentNote()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(Consts.nonexistentNoteUrl);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task GetHistoryById_ReturnsOkAndProperContentType_ForProperId()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(Consts.properHistoryIdUrl);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var notes = _dbContext.Set<Note>().Where(n => n.OriginalNoteId == Consts.originalNoteId);
            Assert.Equal(Consts.numberOfNotesInHistory, notes.Count());
        }

        [Fact]
        public async Task GetHistoryById_ReturnsBadRequestAndProperContentType_ForNonexistentNote()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(Consts.nonexistentNoteHistoryUrl);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Update_ReturnsOkUpdatesAndAddsToDb_ForProperNote()
        {
            // Arrange
            var client = _factory.CreateClient();
            var note = new NoteForUpdatingDto() { Title = "title Y", Content = "content Y" };
            var noteJson = JsonSerializer.Serialize(note);
            var httpContent = new StringContent(noteJson, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await client.PutAsync(Consts.properIdUrl, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var newNote = await _dbContext.FindAsync<Note>(Consts.newNoteId);
            Assert.True(newNote != null);
            Assert.Equal(3, newNote.Version); // The highest version of this note in test db is 2
            var updatedNote = await _dbContext.FindAsync<Note>(1);
            Assert.Equal(updatedNote.Modified.GetDateTimeFormats('G').FirstOrDefault(), newNote.Created.GetDateTimeFormats('G').FirstOrDefault()); // G format = 15/06/2009 13:45:30
        }

        [Fact]
        public async Task Update_ReturnsBadRequestAndProperContentType_ForNonexistentNote()
        {
            // Arrange
            var client = _factory.CreateClient();
            var note = new NoteForUpdatingDto() { Title = "title X", Content = "content X" };
            var noteJson = JsonSerializer.Serialize(note);
            var httpContent = new StringContent(noteJson, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await client.PutAsync(Consts.nonexistentNoteUrl, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.True(await _dbContext.FindAsync<Note>(Consts.nonexistentNoteId) == null);
        }

        [Fact]
        public async Task Update_ReturnsBadRequestAndProperContentTypeDoesntAddAndUpdate_ForDeletedNote()
        {
            // Arrange
            var client = _factory.CreateClient();
            var note = new NoteForUpdatingDto() { Title = "title X", Content = "content X" };
            var noteJson = JsonSerializer.Serialize(note);
            var httpContent = new StringContent(noteJson, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act
            var response = await client.PutAsync(Consts.deletedNoteUrl, httpContent);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var deletedNote = await _dbContext.FindAsync<Note>(Consts.deletedNoteId);
            Assert.True(deletedNote.Title != "title X");
            var newNote = await _dbContext.FindAsync<Note>(Consts.newNoteId);
            Assert.True(newNote == null);
        }

        [Fact]
        public async Task Delete_ReturnsOkAndDeletesNote_ForProperId()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync(Consts.properIdUrl);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var deletedNote = await _dbContext.FindAsync<Note>(1);
            Assert.True(deletedNote.Deleted == true);

        }

        [Fact]
        public async Task Delete_ReturnsBadRequestAndProperContentType_ForNonexistentNote()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync(Consts.nonexistentNoteUrl);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Delete_ReturnsBadRequestAndProperContentType_ForDeletedNote()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.DeleteAsync(Consts.deletedNoteUrl);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("text/plain; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
