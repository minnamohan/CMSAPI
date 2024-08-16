using CMSAPI.Models.DTOs;

namespace CMSAPI.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetCustomers();
        Task<CustomerDto> GetCustomerById(Guid customerId);
        Task<Guid> CreateCustomer(CustomerDto? customerDto);
        Task<bool> UpdateCustomer(Guid customerId, CustomerDto customerDto);
        Task<bool> DeleteCustomer(Guid customerId);
    }
}
