using Redis.OM;
using server.HostedServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// for swagger generator
builder.Services.AddSwaggerGen();

// di for redis connection, not ioc, because no interface
builder.Services.AddSingleton(new RedisConnectionProvider(builder.Configuration["REDIS_CONNECTION_STRING"]));
builder.Services.AddHostedService<IndexCreationService>();
// bind the Position section and add it to the dependency injection service container. 
// In the following code, PositionOptions is added to the service container with Configure and bound to configuration.
builder.Services.Configure<PositionOptions>(builder.Configuration.GetSection(PositionOptions.Position));


// configures a host with a set of default options
// server: Kestrel cross-platform server implementation. 
// Kestrel can run as a public-facing edge server exposed directly to the Internet.
// Kestrel is often run in a reverse proxy configuration with Nginx or Apache.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // a Swagger object model and middleware to expose SwaggerDocument objects as JSON endpoints.
    app.UseSwagger();
    // an embedded version of the Swagger UI tool. It interprets Swagger JSON to build a rich, 
    // customizable experience for describing the web API functionality.
    // It includes built-in test harnesses for the public methods.
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // HTTP Strict Transport Security Protocol (HSTS)
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
