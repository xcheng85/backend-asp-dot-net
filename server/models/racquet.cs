using Redis.OM.Modeling;

namespace server.Model; // define class in the namespace

public class Racquet
{
    [Searchable]
    public string? Brand { get; set; }

    [Searchable]
    public string? Name { get; set; }

    [Searchable]
    public int? HeaderSize { get; set; }

    [Indexed]
    public int? Weight { get; set; }

    [Searchable]
    public int? Price { get; set; }
}