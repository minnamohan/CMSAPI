using AutoMapper;
using CMSAPI.Data;
using CMSAPI.Interfaces;
using CMSAPI.MappingProfiles;
using CMSAPI.Models.DTOs;
using CMSAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMSAPI.Services
{
    public class CustomerService : ICustomerService
    {
        readonly AppDbContext _appDbContext;
        readonly IMapper _mapper;
        public CustomerService(AppDbContext appDbContext, IMapper mapper)
        {
            this._appDbContext = appDbContext;
            this._mapper = mapper;
        }

        public async Task<List<CustomerDto>> GetCustomers()
        {
            var result = new List<CustomerDto>();
            try
            {
                result = _mapper.Map<List<CustomerDto>>(await _appDbContext.Customers.ToListAsync());

            }
            catch (Exception ex)
            {
                throw new Exception($"An exception occurred while retrieving customers {ex}");
            }
            return result;

        }
        public async Task<CustomerDto> GetCustomerById(Guid customerId)
        {
            var customer = new Customer();
            try
            {
               customer = await _appDbContext.Customers.FindAsync(customerId);
            } catch (Exception ex)
            {
                throw new Exception($"An exception occurred while retrieving customer {ex}");
            }
            return _mapper.Map<CustomerDto>(customer);

        }
        public async Task<Guid> CreateCustomer(CustomerDto? customerDto)
        {
            var customer = new Customer();
            if (customerDto == null) throw new ArgumentNullException(nameof(customerDto));
            if (await IsEmailOrPhoneNumberTaken(customerDto.Email, customerDto.PhoneNumber))
            {
                throw new InvalidOperationException("Email or phone number already in use.");
            }
            try
            {              
                customer = _mapper.Map<Customer>(customerDto);
                _appDbContext.Customers.Add(customer);
                await _appDbContext.SaveChangesAsync();
            } 
            catch (Exception ex)
            {
                throw new Exception($"An exception occurred while saving customer {ex}");
            }

            return customer.CustomerId;
        }
        public async Task<bool> UpdateCustomer(Guid customerId, CustomerDto customerDto)
        {
            var customer = new Customer();
            try
            {
                customer = await _appDbContext.Customers.FindAsync(customerId);
                if (customer == null) return false;

                _mapper.Map(customerDto, customer);
                _appDbContext.Customers.Update(customer);
                await _appDbContext.SaveChangesAsync();

            }  
            catch (Exception ex)
            {
                throw new Exception($"An exception occurred while updating customer {ex}");
            }            

            return true;
        }
        private async Task<bool> IsEmailOrPhoneNumberTaken(string? email, string? phoneNumber, Guid? excludedCustomerId = null)
        {
            return await _appDbContext.Customers
                .AnyAsync(c => (c.Email == email || c.PhoneNumber == phoneNumber) &&
                               (excludedCustomerId == null || c.CustomerId != excludedCustomerId));
        }
        public async Task<bool> DeleteCustomer(Guid customerId)
        {
            var customer = new Customer();
            try
            {
                customer = await _appDbContext.Customers.FindAsync(customerId);
                if (customer == null) return false;

                _appDbContext.Customers.Remove(customer);
                await _appDbContext.SaveChangesAsync();
            } catch (Exception ex)
            {
                throw new Exception($"An exception occurred while deleting customer {ex}");
            }

            return true;
        }
    }
}
