using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ConferenceBooking.Application.DTOs.ConferenceRoom;
using ConferenceBooking.Infrastructure.Data;

namespace ConferenceBooking.Tests.Integration.Controllers
{
    public class ConferenceRoomsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ConferenceRoomsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Замінити SQLite на InMemory для тестів
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseInMemoryDatabase("TestDb_Rooms"));
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetAll_ReturnsOk()
        {
            var response = await _client.GetAsync("/api/conferencerooms");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreated()
        {
            var dto = new CreateConferenceRoomDto
            {
                Name = "Integration Test Room",
                Capacity = 25,
                BaseHourlyRate = 1500
            };

            var response = await _client.PostAsJsonAsync("/api/conferencerooms", dto);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var room = await response.Content.ReadFromJsonAsync<ConferenceRoomResponseDto>();
            Assert.NotNull(room);
            Assert.Equal("Integration Test Room", room!.Name);
        }

        [Fact]
        public async Task GetById_NotFound_Returns404()
        {
            var response = await _client.GetAsync($"/api/conferencerooms/{Guid.NewGuid()}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
