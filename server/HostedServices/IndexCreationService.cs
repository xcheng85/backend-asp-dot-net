using Microsoft.Extensions.Options;
using Redis.OM;
using server.Model;

namespace server.HostedServices;

public class IndexCreationService : IHostedService
{
    private readonly RedisConnectionProvider _provider;
    private readonly PositionOptions _options;
    private readonly ILogger _logger;
    private readonly IConfiguration _config;
    public IndexCreationService(RedisConnectionProvider provider, IOptions<PositionOptions> options,
    ILogger<IndexCreationService> logger, IConfiguration config)
    {
        _provider = provider;
        _options = options.Value;
        _logger = logger;
        _config = config;

        _logger.LogInformation($"Title: {_options.Title} \n" + $"Name: {_options.Name}");
        _logger.LogInformation(_config["Movies:ServiceApiKey"]);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _provider.Connection.CreateIndexAsync(typeof(Player));
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}