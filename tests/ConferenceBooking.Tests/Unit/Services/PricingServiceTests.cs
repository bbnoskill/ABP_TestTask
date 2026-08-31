using Xunit;
using Moq;
using ConferenceBooking.Application.Services;
using ConferenceBooking.Domain.Enums;

namespace ConferenceBooking.Tests.Unit.Services
{
    public class PricingServiceTests
    {
        private readonly PricingService _pricingService = new();

        [Theory]
        [InlineData(6, TimeSlotType.Morning)]
        [InlineData(8, TimeSlotType.Morning)]
        [InlineData(9, TimeSlotType.Standard)]
        [InlineData(11, TimeSlotType.Standard)]
        [InlineData(12, TimeSlotType.Peak)]
        [InlineData(13, TimeSlotType.Peak)]
        [InlineData(14, TimeSlotType.Standard)]
        [InlineData(18, TimeSlotType.Evening)]
        [InlineData(22, TimeSlotType.Evening)]
        public void GetTimeSlotType_ReturnsCorrectType(int hour, TimeSlotType expected)
        {
            var result = _pricingService.GetTimeSlotType(hour);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(TimeSlotType.Morning, 0.9)]
        [InlineData(TimeSlotType.Standard, 1.0)]
        [InlineData(TimeSlotType.Peak, 1.15)]
        [InlineData(TimeSlotType.Evening, 0.8)]
        public void GetPriceCoefficient_ReturnsCorrectValue(TimeSlotType type, decimal expected)
        {
            var result = _pricingService.GetPriceCoefficient(type);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CalculateTotalPrice_StandardHours_ReturnsBaseRate()
        {
            // 10:00-11:00, 1 година стандарт, ставка 1000
            var result = _pricingService.CalculateTotalPrice(
                1000m,
                new DateTime(2025, 1, 1, 10, 0, 0),
                new DateTime(2025, 1, 1, 11, 0, 0));

            Assert.Equal(1000m, result);
        }

        [Fact]
        public void CalculateTotalPrice_PeakHours_AppliesCoefficient()
        {
            // 12:00-14:00, 2 години пік, ставка 1000
            var result = _pricingService.CalculateTotalPrice(
                1000m,
                new DateTime(2025, 1, 1, 12, 0, 0),
                new DateTime(2025, 1, 1, 14, 0, 0));

            Assert.Equal(2300m, result); // 1000 * 1.15 * 2
        }

        [Fact]
        public void CalculateTotalPrice_MixedSlots_CalculatesCorrectly()
        {
            // 11:00-13:00: 1h Standard (1.0) + 1h Peak (1.15), ставка 2000
            var result = _pricingService.CalculateTotalPrice(
                2000m,
                new DateTime(2025, 1, 1, 11, 0, 0),
                new DateTime(2025, 1, 1, 13, 0, 0));

            Assert.Equal(4300m, result); // 2000*1.0 + 2000*1.15
        }

        [Fact]
        public void CalculateTotalPrice_WithServices_AddsServiceCost()
        {
            // 10:00-11:00 Standard + послуги 500 + 300
            var result = _pricingService.CalculateTotalPrice(
                1000m,
                new DateTime(2025, 1, 1, 10, 0, 0),
                new DateTime(2025, 1, 1, 11, 0, 0),
                new[] { 500m, 300m });

            Assert.Equal(1800m, result); // 1000 + 500 + 300
        }

        [Fact]
        public void CalculateTotalPrice_EveningHours_AppliesDiscount()
        {
            // 19:00-21:00, 2 години вечір, ставка 1000
            var result = _pricingService.CalculateTotalPrice(
                1000m,
                new DateTime(2025, 1, 1, 19, 0, 0),
                new DateTime(2025, 1, 1, 21, 0, 0));

            Assert.Equal(1600m, result); // 1000 * 0.8 * 2
        }
    }
}
