using Redis.OM.Modeling;

namespace server.Model; // define class in the namespace

// namespace Redis.OM.Skeleton.Model
// use one prefix (keyspace) Player
[Document(StorageType = StorageType.Json, Prefixes = new[] { "Player" })]
public class Player
{
    // generate the document's key name when it's stored in Redis.
    [RedisIdField] [Indexed]
    public string? Id { get; set; }

    [Searchable]
    public string? FirstName { get; set; }

    [Searchable]
    public string? LastName { get; set; }

    [Indexed]
    public int? ProYear { get; set; }

    [Indexed]
    public int? Weight { get; set; }

    [Indexed]
    public int? Height { get; set; }

    [Indexed]
    public string? City { get; set; }

    [Indexed]
    public string? Country { get; set; }

    [Indexed]
    public string[]? Coaches { get; set; }

    [Indexed(CascadeDepth = 1)]
    public Racquet? Racquet { get; set; }
}