using CMSAPI.Interfaces;
using CMSAPI.Models.DTOs;
using CMSAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CMSAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Gets all Customers
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDto))]       
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            return Ok(await _customerService.GetCustomers());
        }

        /// <summary>
        /// Retrieves a customer by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(Guid id)
        {
            var customer = await _customerService.GetCustomerById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customerDto">The customer details to be created.</param>
        /// <response code="201">Customer created successfully.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<int>> PostCustomer(CustomerDto customerDto)
        {
            var customerId = await _customerService.CreateCustomer(customerDto);
            return CreatedAtAction(nameof(GetCustomer), new { id = customerId }, customerId);
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="customerDto">The updated customer details.</param>
        /// <response code="204">Customer updated successfully.</response>
        /// <response code="400">If the ID in the URL does not match the ID in the request body.</response>
        /// <response code="404">If the customer is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCustomer(Guid id,CustomerDto customerDto)
        {
            if (id != customerDto.CustomerId)
            {
                return BadRequest();
            }
            var result = await _customerService.UpdateCustomer(id,customerDto);
            if (!result)
            {
                return NotFound();
            }

            return Ok("Successfully Updated");
        }

        /// <summary>
        /// Deletes an existing customer.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <response code="204">Customer deleted successfully.</response>
        /// <response code="404">If the customer is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var result = await _customerService.DeleteCustomer(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok("Successfully Deleted");
        }
    }
}
    