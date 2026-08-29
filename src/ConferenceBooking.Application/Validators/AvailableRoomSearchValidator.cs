using FluentValidation;
using ConferenceBooking.Application.DTOs.Search;

namespace ConferenceBooking.Application.Validators
{
    /// <summary>Валідатор пошуку доступних залів.</summary>
    public class AvailableRoomSearchValidator : AbstractValidator<AvailableRoomSearchDto>
    {
        public AvailableRoomSearchValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Дата обов'язкова.")
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                    .WithMessage("Пошук можливий лише на поточну або майбутню дату.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage("Час завершення повинен бути пізніше часу початку.");

            RuleFor(x => x.Capacity)
                .GreaterThan(0).WithMessage("Місткість повинна бути більше 0.");
        }
    }
}
