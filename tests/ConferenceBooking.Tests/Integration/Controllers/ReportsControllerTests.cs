using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ConferenceBooking.Infrastructure.Data;

namespace ConferenceBooking.Tests.Integration.Controllers
{
    public class ReportsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ReportsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseInMemoryDatabase("TestDb_Reports"));
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetRoomUsage_ValidDates_ReturnsOk()
        {
            var response = await _client.GetAsync(
                "/api/reports/room-usage?startDate=2025-01-01&endDate=2025-12-31");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetRevenue_ValidDates_ReturnsOk()
        {
            var response = await _client.GetAsync(
                "/api/reports/revenue?startDate=2025-01-01&endDate=2025-12-31");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetPopularServices_ValidDates_ReturnsOk()
        {
            var response = await _client.GetAsync(
                "/api/reports/popular-services?startDate=2025-01-01&endDate=2025-12-31");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetRoomUsage_InvalidDates_ReturnsBadRequest()
        {
            var response = await _client.GetAsync(
                "/api/reports/room-usage?startDate=2025-12-31&endDate=2025-01-01");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
