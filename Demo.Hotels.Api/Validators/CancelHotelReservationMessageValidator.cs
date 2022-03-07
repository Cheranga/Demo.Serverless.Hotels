using Demo.Hotels.Api.DTO.Messages;
using FluentValidation;

namespace Demo.Hotels.Api.Validators
{
    public class CancelHotelReservationMessageValidator : ModelValidatorBase<CancelHotelReservationMessage>
    {
        public CancelHotelReservationMessageValidator()
        {
            RuleFor(x => x.CorrelationId).NotNull().NotEmpty();
            RuleFor(x => x.ReservationId).NotNull().NotEmpty();
            RuleFor(x => x.UserId).InclusiveBetween(1, 10);
        }
    }
}