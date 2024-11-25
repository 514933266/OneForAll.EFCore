using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;

namespace UnitTest.Host
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OrderRepository : Repository<Order>
    {
        public OrderRepository(List<DbContext> contexts)
            : base(contexts)
        {

        }
    }
}

    
