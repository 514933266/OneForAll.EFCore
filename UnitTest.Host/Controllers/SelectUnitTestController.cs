using Microsoft.AspNetCore.Mvc;
using OneForAll.EFCore;

namespace UnitTest.Host.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class SelectUnitTestController : ControllerBase
    {
        private readonly OrderRepository _repository;
        public SelectUnitTestController(OrderRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> GetAsync()
        {
            var data = await _repository.GetListAsync(w => w.Id == 1);
            if (!data.Any())
            {
                var effected = await _repository.AddAsync(new Order { ProductName = "订单", TotalPrice = 0.01m, CreateTime = DateTime.Now });
                if (effected > 0)
                    data = await _repository.Readonly.GetListAsync(w => w.Id == 1);
            }
            return data;

            using (var tran = new UnitOfWork().BeginTransaction())
            {
                await _repository.AddAsync(new Order { ProductName = "订单", TotalPrice = 0.01m, CreateTime = DateTime.Now }, tran);
                await _repository2.AddAsync(new Order { ProductName = "订单明细", TotalPrice = 0.01m, CreateTime = DateTime.Now }, tran);
                var effected = tran.Commit();
            }
    }
}
