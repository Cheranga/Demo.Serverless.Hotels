using System.Threading.Tasks;
using Demo.Hotels.Api.Core;
using Demo.Hotels.Api.DTO.Messages;

namespace Demo.Hotels.Api.Services
{
    public interface ICancelHotelReservationService
    {
        Task<Result> CancelAsync(CancelHotelReservationMessage request);
    }
}