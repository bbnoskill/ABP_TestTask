using Xunit;
using FluentValidation.TestHelper;
using ConferenceBooking.Application.Validators;
using ConferenceBooking.Application.DTOs.ConferenceRoom;

namespace ConferenceBooking.Tests.Unit.Validators
{
    public class CreateConferenceRoomValidatorTests
    {
        private readonly CreateConferenceRoomValidator _validator = new();

        [Fact]
        public void ValidDto_NoErrors()
        {
            var dto = new CreateConferenceRoomDto
            {
                Name = "Зал А",
                Capacity = 50,
                BaseHourlyRate = 2000
            };

            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void EmptyName_HasError()
        {
            var dto = new CreateConferenceRoomDto { Name = "", Capacity = 50, BaseHourlyRate = 2000 };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void ZeroCapacity_HasError()
        {
            var dto = new CreateConferenceRoomDto { Name = "Зал", Capacity = 0, BaseHourlyRate = 2000 };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Capacity);
        }

        [Fact]
        public void NegativeRate_HasError()
        {
            var dto = new CreateConferenceRoomDto { Name = "Зал", Capacity = 50, BaseHourlyRate = -1 };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.BaseHourlyRate);
        }
    }
}
