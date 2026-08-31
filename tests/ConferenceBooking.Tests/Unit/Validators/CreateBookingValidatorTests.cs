using Xunit;
using FluentValidation.TestHelper;
using ConferenceBooking.Application.Validators;
using ConferenceBooking.Application.DTOs.Booking;

namespace ConferenceBooking.Tests.Unit.Validators
{
    public class CreateBookingValidatorTests
    {
        private readonly CreateBookingValidator _validator = new();

        [Fact]
        public void ValidDto_NoErrors()
        {
            var dto = new CreateBookingDto
            {
                RoomId = Guid.NewGuid(),
                StartDateTime = DateTime.UtcNow.AddHours(1),
                EndDateTime = DateTime.UtcNow.AddHours(3)
            };

            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void EmptyRoomId_HasError()
        {
            var dto = new CreateBookingDto
            {
                RoomId = Guid.Empty,
                StartDateTime = DateTime.UtcNow.AddHours(1),
                EndDateTime = DateTime.UtcNow.AddHours(3)
            };

            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.RoomId);
        }

        [Fact]
        public void EndBeforeStart_HasError()
        {
            var dto = new CreateBookingDto
            {
                RoomId = Guid.NewGuid(),
                StartDateTime = DateTime.UtcNow.AddHours(3),
                EndDateTime = DateTime.UtcNow.AddHours(1)
            };

            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.EndDateTime);
        }
    }
}
