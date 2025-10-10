var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 
builder.Services.AddLogging(); 


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "RedisCacheApi_";
});

builder.Services.AddScoped<IUserService, UserService>(); 
builder.Services.AddScoped(typeof(ICacheRepository<>), typeof(CacheRepository<>));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); 

app.Run();