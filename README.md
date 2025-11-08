# OneForAll.EFCore

### v1.0.2.1 支持单列动态Order by
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