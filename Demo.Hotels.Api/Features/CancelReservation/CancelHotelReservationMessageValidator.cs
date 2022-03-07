using Demo.Hotels.Api.Core;
using FluentValidation;

namespace Demo.Hotels.Api.Features.CancelReservation
{
    public class CancelHotelReservationMessageValidator : ModelValidatorBase<CancelHotelReservationMessage>
    {
        public CancelHotelReservationMessageValidator()
        {
            RuleFor(x => x.CorrelationId).NotNull().NotEmpty();
            RuleFor(x => x.ReservationId).NotNull().NotEmpty();
            RuleFor(x => x.UserId).GreaterThanOrEqualTo(1);
        }
    }
}