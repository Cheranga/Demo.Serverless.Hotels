using System.Threading.Tasks;
using Demo.Hotels.Api.DTO.Requests;
using Demo.Hotels.Api.DTO.Responses;

namespace Demo.Hotels.Api.Application
{
    public interface ICustomerService
    {
        Task<GetCustomerResponse> GetCustomer(GetCustomerRequest request);
    }
}