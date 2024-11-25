using Microsoft.AspNetCore.Mvc;

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
            var data = await _repository.Readonly.GetListAsync(w => w.Id == 1);
            if (!data.Any())
            {
                var effected = await _repository.AddAsync(new Order { ProductName = "²âÊÔ¶©µ¥", TotalPrice = 0.01m, CreateTime = DateTime.Now });
                if (effected > 0)
                    data = await _repository.Readonlys[0].GetListAsync(w => w.Id == 1);
            }
            return data;
        }
    }
}
