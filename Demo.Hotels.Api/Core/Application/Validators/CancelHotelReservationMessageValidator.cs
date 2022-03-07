using Demo.Hotels.Api.Core.Domain.Messages;
using FluentValidation;

namespace Demo.Hotels.Api.Core.Application.Validators
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