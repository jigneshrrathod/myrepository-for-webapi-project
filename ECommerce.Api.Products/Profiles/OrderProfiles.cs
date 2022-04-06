using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Profiles
{
    public class OrderProfiles : AutoMapper.Profile
    {
        public OrderProfiles()
        {
            CreateMap<Db.Order, Models.Order>();
        }
    }
}
