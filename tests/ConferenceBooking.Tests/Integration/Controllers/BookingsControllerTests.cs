using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ConferenceBooking.Application.DTOs.Booking;
using ConferenceBooking.Application.DTOs.ConferenceRoom;
using ConferenceBooking.Infrastructure.Data;

namespace ConferenceBooking.Tests.Integration.Controllers
{
    public class BookingsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BookingsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseInMemoryDatabase("TestDb_Bookings"));
                });
            }).CreateClient();
        }

        [Fact]
        public async Task CreateBooking_InvalidRoomId_ReturnsBadRequestOrNotFound()
        {
            var dto = new CreateBookingDto
            {
                RoomId = Guid.NewGuid(),
                StartDateTime = DateTime.UtcNow.AddDays(1),
                EndDateTime = DateTime.UtcNow.AddDays(1).AddHours(2)
            };

            var response = await _client.PostAsJsonAsync("/api/bookings", dto);

            // Domain exception mapped via middleware — 404 (room not found)
            Assert.True(
                response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetById_NotFound_Returns404()
        {
            var response = await _client.GetAsync($"/api/bookings/{Guid.NewGuid()}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
