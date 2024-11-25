using Microsoft.EntityFrameworkCore;
using System.Data;
using UnitTest.Host;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TestDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("Default");
    options.UseSqlServer(conn);
});

builder.Services.AddDbContext<TestSlaveDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("Slave");
    options.UseSqlServer(conn);
});

builder.Services.AddTransient<OrderRepository>(provider =>
{
    var context1 = provider.GetRequiredService<TestDbContext>();
    var context2 = provider.GetRequiredService<TestSlaveDbContext>();
    return new OrderRepository(new List<DbContext>()
    {
        context1,context2
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
