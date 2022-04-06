using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext _db;
        private readonly ILogger<CustomersProvider> _logger;
        private readonly IMapper _mapper;

        public CustomersProvider(CustomersDbContext db, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;

            SeedData();
        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await _db.Customers.FirstOrDefaultAsync(p => p.Id == id);
                if (customer != null)
                {
                    var result = _mapper.Map<Db.Customer, Models.Customer>(customer);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await _db.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    var result = _mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        private void SeedData()
        {
            if (!_db.Customers.Any())
            {
                _db.Customers.Add(new Db.Customer() { Id = 1, Name = "Rachel",Address = "At Monica's House" });
                _db.Customers.Add(new Db.Customer() { Id = 2, Name = "Ross", Address = "Own House" });
                _db.Customers.Add(new Db.Customer() { Id = 3, Name = "Chandler", Address = "Own house" });
                _db.Customers.Add(new Db.Customer() { Id = 4, Name = "Joey", Address = "At Chandler's House" });
                _db.SaveChanges();
            }
        }

        
    }
}
