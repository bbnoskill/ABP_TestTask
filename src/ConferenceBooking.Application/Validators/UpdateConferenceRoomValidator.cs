using FluentValidation;
using ConferenceBooking.Application.DTOs.ConferenceRoom;

namespace ConferenceBooking.Application.Validators
{
    /// <summary>Валідатор оновлення конференц-залу.</summary>
    public class UpdateConferenceRoomValidator : AbstractValidator<UpdateConferenceRoomDto>
    {
        public UpdateConferenceRoomValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Назва не може перевищувати 100 символів.")
                .When(x => x.Name != null);

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Місткість повинна бути більше 0.")
                .LessThanOrEqualTo(1000).WithMessage("Місткість не може перевищувати 1000 осіб.")
                .When(x => x.Capacity.HasValue);

            RuleFor(x => x.BaseHourlyRate)
                .GreaterThan(0).WithMessage("Базова вартість повинна бути більше 0 грн.")
                .When(x => x.BaseHourlyRate.HasValue);
        }
    }
}
