# OneForAll.EFCore

<br>
本框架为基于EF Core进行二次封装的ORM，用于帮助开发人员快速实现仓储层相关功能（增、删、查、改、工作单元、跨库事务、读写分离）
<br>
由于EF已经相当完善且有微软官方团队全力维护，因此本框架不会过度封装（整体用法与EF保持高度一致），因此，与其他框架不同的是可以避免更多由于EF更新导致的冲突问题，可以放心使用

### 通过Autofac注入

```C#

builder.Register(p =>
{
    var optionBuilder = new DbContextOptionsBuilder<SysDbContext>();
    optionBuilder.UseSqlServer(Configuration["ConnectionStrings:Default"]);
    return optionBuilder.Options;
}).AsSelf();
builder.RegisterType(typeof(SysDbContext)).Named<DbContext>("SysDbContext");
builder.RegisterAssemblyTypes(Assembly.Load(BASE_REPOSITORY))
               .Where(t => t.Name.EndsWith("Repository"))
               .WithParameter(ResolvedParameter.ForNamed<DbContext>("SysDbContext"))
               .AsImplementedInterfaces();

```

### 基础用法

#### 新建仓储类

```C#

public class OrderRepository : Repository<Order>, IOrderRepository
{
        public OrderRepository(DbContext context)
            : base(context)
        {

        }
}

```

### 单列动态Order by

<br>

```C#
using OneForAll.EFCore;

string sortField = "Id"
string sortType = "desc"
var items = await DbSet
                .Include(e => e.Order)
                .OrderBy(sortField, sortType)
                .ThenByDescending(e => e.CreateTime)
                .ToListAsync();
```

### 事务
```C#
using (var tran = new UnitOfWork().BeginTransaction())
{
    await _repository.AddAsync(new Order { ProductName = "订单", TotalPrice = 0.01m, CreateTime = DateTime.Now }, tran);
    await _repository2.AddAsync(new Order { ProductName = "订单明细", TotalPrice = 0.01m, CreateTime = DateTime.Now }, tran);
    var effected = tran.Commit();
}
```
<br>


### 读写分离

```C#

// 1. 依赖注入多个DbContext
builder.Services.AddTransient<OrderRepository>(provider =>
{
    var context1 = provider.GetRequiredService<TestDbContext>();
    var context2 = provider.GetRequiredService<TestSlaveDbContext>();
    return new OrderRepository(new List<DbContext>()
    {
        context1,context2
    });
});

// 2. 使用读库
var data = await _repository.Readonly.GetListAsync(w => w.Id == 1);

// 3. 使用指定读库
var data = await _repository.Readonlys[0].GetListAsync(w => w.Id == 1);

```