using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CMSAPI.Models.Entities;
using CMSAPI.Data;
using CMSAPI.Services;
using CMSAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CMSAPI.Tests
{
    public class CustomerServiceTests : IDisposable
    {
        private readonly CustomerService _customerService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CustomerServiceTests()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(DatabaseSetup.GetTestSqlServerConnectionString())
                .EnableSensitiveDataLogging();

            _context = new AppDbContext(optionsBuilder.Options);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDto>();
                cfg.CreateMap<CustomerDto, Customer>();
            });
            _mapper = new Mapper(mapperConfig);

            _customerService = new CustomerService(_context, _mapper);

            // Ensure the database is created
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetCustomers_ShouldReturnCustomerList()
        {
            // Arrange
            var customer1 = new Customer { CustomerId = Guid.NewGuid(), FirstName = "xx", LastName = "xy",Email = "xxxx@gmail.com" };
            var customer2 = new Customer { CustomerId = Guid.NewGuid(), FirstName = "aa", LastName = "bb", Email = "xxyx@gmail.com" };
            _context.Customers.AddRange(customer1, customer2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.GetCustomers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.FirstName == "xx" && c.LastName == "xy" && c.Email == "xxxx@gmail.com");
            Assert.Contains(result, c => c.FirstName == "aa" && c.LastName == "bb" && c.Email == "xxyx@gmail.com");
        }

        [Fact]
        public async Task GetCustomerById_ShouldReturnCustomer()
        {
            // Arrange
            var customer = new Customer { CustomerId = Guid.NewGuid(), FirstName = "xx", LastName = "xy", Email = "xxyx@gmail.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.GetCustomerById(customer.CustomerId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("xx", result.FirstName);
            Assert.Equal("xy", result.LastName);
        }

        [Fact]
        public async Task CreateCustomer_ShouldAddCustomer()
        {
            // Arrange
            var customerDto = new CustomerDto(
                                            Guid.Empty,
                                            "xx",
                                            "yy",
                                            "xx.yy@gmail.com",
                                            "1234567890",
                                            "abc street"
                                        );

            // Act
            var customerId = await _customerService.CreateCustomer(customerDto);

            // Assert
            var customer = await _context.Customers.FindAsync(customerId);
            Assert.NotNull(customer);
            Assert.Equal("xx", customer.FirstName);
            Assert.Equal("yy", customer.LastName);
            Assert.Equal("xx.yy@gmail.com", customer.Email);
            Assert.Equal("abc street", customer.Address);
            Assert.Equal("1234567890", customer.PhoneNumber);
        }

        [Fact]
        public async Task UpdateCustomer_ShouldModifyCustomer()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer { CustomerId = customerId, FirstName = "xx", LastName = "xy", Email = "xxxx@gmail.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var customerDto = new CustomerDto
                                (customerId,
                                "yy",
                                "xx",
                                "yyyy@gmail.com",
                                "1234567890",
                                "abc street"
                                );

            // Act
            var result = await _customerService.UpdateCustomer(customer.CustomerId, customerDto);

            // Assert
            Assert.True(result);
            var updatedCustomer = await _context.Customers.FindAsync(customer.CustomerId);
            Assert.Equal("yy", updatedCustomer.FirstName);
            Assert.Equal("xx", updatedCustomer.LastName);
            Assert.Equal("yyyy@gmail.com", updatedCustomer.Email);
        }

        [Fact]
        public async Task DeleteCustomer_ShouldRemoveCustomer()
        {
            // Arrange
            var customer = new Customer { CustomerId = Guid.NewGuid(), FirstName = "xx", LastName = "xy", Email = "xxxx@gmail.com" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.DeleteCustomer(customer.CustomerId);

            // Assert
            Assert.True(result);
            var deletedCustomer = await _context.Customers.FindAsync(customer.CustomerId);
            Assert.Null(deletedCustomer);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
