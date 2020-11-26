using UsedProductExchange.Infrastructure.Context;

namespace UsedProductExchange.Infrastructure.DBInitializer
{
    public interface IDbInitializer
    {
        void Initialize(UsedProductExchangeContext context);
    }
}