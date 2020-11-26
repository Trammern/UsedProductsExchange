using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Infrastructure.Context;

namespace UsedProductExchange.Infrastructure.DBInitializer
{
   public class DBInitializer
    {
        
        
        private readonly IRepository _repository;
        private readonly UsedProductExchangeContext _ctx;

        public DBInitializer(
                 UsedProductExchangeContext ctx,
                
            _ctx = ctx;
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _cityRepository = cityRepository;
            _countryRepository = countryRepository;
        }
    }
}
