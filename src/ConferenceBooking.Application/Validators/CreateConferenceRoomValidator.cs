using FluentValidation;
using ConferenceBooking.Application.DTOs.ConferenceRoom;

namespace ConferenceBooking.Application.Validators
{
    /// <summary>Валідатор створення конференц-залу.</summary>
    public class CreateConferenceRoomValidator : AbstractValidator<CreateConferenceRoomDto>
    {
        public CreateConferenceRoomValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва залу обов'язкова.")
                .MaximumLength(100).WithMessage("Назва не може перевищувати 100 символів.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Місткість повинна бути більше 0.")
                .LessThanOrEqualTo(1000).WithMessage("Місткість не може перевищувати 1000 осіб.");

            RuleFor(x => x.BaseHourlyRate)
                .GreaterThan(0).WithMessage("Базова вартість повинна бути більше 0 грн.");
        }
    }
}
