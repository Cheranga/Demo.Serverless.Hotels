using System.Threading.Tasks;
using Demo.Hotels.Api.Core;

namespace Demo.Hotels.Api.Features.CancelReservation
{
    public interface ICancelHotelReservationService
    {
        Task<Result> CancelAsync(CancelHotelReservationMessage request);
    }
}