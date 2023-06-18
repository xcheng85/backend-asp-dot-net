using Microsoft.AspNetCore.Mvc;
using Redis.OM.Searching;
using Redis.OM;
using server.Model;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    // RedisCollection<Person> instance, which will allow a fluent interface for querying documents in Redis.
    private readonly RedisCollection<Player> _player;

    private readonly RedisConnectionProvider _provider;
    public PlayerController(RedisConnectionProvider provider)
    {
        _provider = provider;
        _player = (RedisCollection<Player>)provider.RedisCollection<Player>();
    }

    [HttpPost]
    public async Task<Player> AddPlayer([FromBody] Player player)
    {
        await _player.InsertAsync(player);
        return player;
    }

    [HttpDelete("{id}")]
    public IActionResult DeletePlayer([FromRoute] string id)
    {
        // see doc: UNLINK VS DEL
        // Player is the prefix
        _provider.Connection.Unlink($"Player:{id}");
        return NoContent();
    }
}