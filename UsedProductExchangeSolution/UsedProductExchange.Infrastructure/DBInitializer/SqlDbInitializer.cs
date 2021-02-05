using System.Collections.Generic;
using UsedProductExchange.Infrastructure.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Infrastructure.DBInitializer
{
    public class SqlDbInitializer: IDbInitializer
    {
        private readonly ILoginService _loginService;
        
        public SqlDbInitializer(ILoginService loginService)
        {
            _loginService = loginService;
        }
        
        public void Initialize(UsedProductExchangeContext context)
        {
             context.Database.EnsureCreated();

        }
    }
}