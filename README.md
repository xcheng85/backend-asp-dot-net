# backend-asp-dot-net


## fundamentals

controller-based APIs: viz-3d-service-k8s style

minimal APIs: viz-3d-service style



## scaffold server
```shell
dotnet new webapi -o server
cd server
dotnet add package Microsoft.EntityFrameworkCore.InMemory
code -r ../server
```

##  run the server
```shell
dotnet run

Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5256
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /home/xiao/backend-asp-dot-net/server

http://localhost:5256/swagger/index.html

```

## redis client without om
NRedisStack is a .NET client for Redis. NredisStack requires a running Redis or Redis Stack server.

## redis om
```install
<!-- The Redis OM client libraries let you use the document modeling, indexing, and querying capabilities of Redis Stack much like the way you'd use an ORM. Object–relational mapping -->

dotnet add package Redis.OM

<!-- info : Package 'Redis.OM' is compatible with all the specified frameworks in project '/home/xiao/backend-asp-dot-net/server/server.csproj'.
info : PackageReference for package 'Redis.OM' version '0.5.2' added to file '/home/xiao/backend-asp-dot-net/server/server.csproj'.
info : Writing assets file to disk. Path: /home/xiao/backend-asp-dot-net/server/obj/project.assets.json -->

```

## config redis connection string

"REDIS_CONNECTION_STRING": "redis://localhost:6379"


## di for redis connection
```shell
builder.Services.AddSingleton(new RedisConnectionProvider(builder.Configuration["REDIS_CONNECTION_STRING"]));

RedisConnectionProvider is in Redis.OM namespace

```

## create HostedServices for all background tasks

### create redis index as background task

```shell

mkdir -p HostedServices && touch IndexCreationService.cs

```

## create model
```shell
mkdir -p models && cd models && touch player.cs racquet.cs

Index and search JSON documents: https://redis.io/docs/stack/search/indexing_json/

DocumentAttribute: 
      1. Redis.OM namespace
      2. c# system attribute class
      3. Two storage type in redis: StorageType.Json or StorageType.Hash
      4. Prefix: check documentation of redisSearch extra modules, keyspace:
      ```shell
         # two prefixes
         cli: FT.CREATE some:index ON JSON PREFIX 2 some:json: some:other:json: SCHEMA ...
         # one prefix
         127.0.0.1:6379> FT.CREATE itemIdx ON JSON PREFIX 1 item: SCHEMA $.name AS name TEXT $.description as description TEXT $.price AS price NUMERIC $.embedding AS embedding VECTOR FLAT 6 DIM 4 DISTANCE_METRIC L2 TYPE FLOAT32
      ```

DocumentAttribute: SearchFieldAttribute : RedisFieldAttribute


### embedded object with Redis OM

1. [Indexed(CascadeDepth = 1)] 

2. JsonPath property
```

# create controller
```shell
mkdir -p controllers && cd controllers && touch PlayerController.cs

```