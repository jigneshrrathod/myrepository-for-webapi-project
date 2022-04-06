using ECommerce.Api.Search.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersServices customersServices;

        public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomersServices customersServices)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customersServices = customersServices;
        }

        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerid)
        {
            var customersResult = await customersServices.GetCustomerAsync(customerid);
            var ordersResult = await ordersService.GetOrdersAsync(customerid);
            var productsResult = await productsService.GetProductsAsync();

            if(ordersResult.IsSuccess)
            {
                foreach (var order in ordersResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess ?
                            productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name :
                            "Product Information is not available";
                    }
                }
                var result = new
                {
                    Customer = customersResult.IsSuccess ?
                                    customersResult.Customer :
                                    new { Name = "Customer Information is not available" },
                    Orders = ordersResult.Orders
                };
                return (true, result);

                //return (true, ordersResult.Orders);
            }

            return (false, null);
        }
    }
}
