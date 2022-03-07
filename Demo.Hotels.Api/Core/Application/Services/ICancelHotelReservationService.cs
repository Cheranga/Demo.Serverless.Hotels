using System.Threading.Tasks;
using Demo.Hotels.Api.Core.Domain.Messages;

namespace Demo.Hotels.Api.Core.Application.Services
{
    public interface ICancelHotelReservationService
    {
        Task<Result> CancelAsync(CancelHotelReservationMessage request);
    }
}