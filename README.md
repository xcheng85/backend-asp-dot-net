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

## run the redis-server
```shell
docker run -p 6379:6379 -p 8001:8001 redis/redis-stack
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

## asp.net startup
EventSource pattern: for tracing cap
source code: 
The ServerReady event in Microsoft.AspNetCore.Hosting 



Apps using EventSource can measure the startup time to understand and optimize startup performance.


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

## configurations
JSON configuration provider


application vs host configurations
Application configuration is the highest priority: WebApplication.
Host configuration follows application configuration: DOTNET_-prefixed environment variables, ASPNETCORE_-prefixed environment variables


.NET Generic vs and Web Host

appsettings.{Environment}.json using the JSON configuration provider. 

auto reload for appsettings.json
reloadOnChange: true. Changes made to the appsettings.json and appsettings.{Environment}.json file after the app starts are read by the JSON configuration provider.


configuration provider: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0

descending priority

1. .net cli: 
```c#
dotnet run MyKey="Using =" Position:Title=Cmd Position:Name=Cmd_Rick
```

2. Non-prefixed environment variables configuration provider.


3. appsettings.json


DI for configurations

### Option pattern: (similar to the work I have done in class-validator)
The options pattern uses classes to provide strongly typed access to groups of related settings.
Encapsulation:
Classes that depend on configuration settings depend only on the configuration settings that they use.
Separation of Concerns:
Settings for different parts of the app aren't dependent or coupled to one another.

use class to mapping json(appSettings.json) + di
An alternative approach when using the options pattern is to bind the Position section and add it to the dependency injection service containe


## secrets

1. .Net Secret Manager for local development

```shell
cd server
dotnet user-secrets init
#Set UserSecretsId to 'ea0d2029-5f92-4e9e-bb56-17c7f9df3259' for MSBuild project '/home/xiao/backend-asp-dot-net/server/server.csproj'.
# check the server.csproj
#<UserSecretsId>ea0d2029-5f92-4e9e-bb56-17c7f9df3259</UserSecretsId>
dotnet user-secrets set "Movies:ServiceApiKey" "12345"
Successfully saved Movies:ServiceApiKey = 12345 to the secret store.

cat ~/.microsoft/usersecrets/ea0d2029-5f92-4e9e-bb56-17c7f9df3259/secrets.json

# access the secrets 
di for IConfiguration config
```


2. For production secrets, we recommend Azure Key Vault.

## logging providers
logging API that works with a variety of built-in and third-party logging providers. 
resolve:
ILogger<TCategoryName> service from dependency injection (DI)

## http client
IHttpClientFactory in ASP.NET Core.

## Content root

## webroot

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

## create controller
```shell
mkdir -p controllers && cd controllers && touch PlayerController.cs
```

## return type for controller method
1. IActionResult: multiple branches of result such as different status code

### model binding for controller
samples: https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/mvc/models/model-binding/samples

sources: ordered
1. Form fields
2. The request body (For controllers that have the [ApiController] attribute.)
3. Route data
4. Query string parameters
5. Uploaded files

6. Additional sources:
```c#
builder.Services.AddControllers(options =>
{
    options.ValueProviderFactories.Add(new CookieValueProviderFactory());
});

### custom model bindings
implementing custom model binders
samples: https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/mvc/advanced/custom-model-binding/samples
use case: For example, when you have a key that can be used to look up model data. You can use a custom model binder to fetch data based on the key.

```

in addition, mannualy define it in the model class
1. [FromQuery] - Gets values from the query string.
2. [FromRoute] - Gets values from route data.
3. [FromForm] - Gets values from posted form fields.
4. [FromBody] - Gets values from the request body.
5. [FromHeader] - Gets values from HTTP headers.


### model validation
samples: https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/mvc/models/validation/samples
two types of errors:
1. model binding: type mismatch
2. model business logic error: out of range
For web apps, it's the app's responsibility to inspect ModelState.IsValid and react appropriately

Web API controllers don't have to check ModelState.IsValid if they have the [ApiController] attribute. In that case, an automatic HTTP 400 response containing error details is returned when model state is invalid

Validation attributes: for model properties

ns: System.ComponentModel.DataAnnotations
1. built-in

      a. required

      b. remote: client-side validation that requires calling a method on the server to determine whether field input is valid, combo of multiple fields for validation: additionalattributes


2. custom: inherit ValidationAttribute
3. Implement IValidatableObject
      a. inher

Q: why model property fields have ?:

A: The validation system treats non-nullable parameters or bound properties as if they had a [Required(AllowEmptyStrings = true)] attribute, or you can disable through
```c#
builder.Services.AddControllers(
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
```

Controller-layer model custom validation
```shell
# in your controller
ModelState.AddModelError(nameof(contact.ShortName),
                                 "Short name can't be the same as Name.");
```

Top node level model validation
1. Action parameters
[BindRequired, FromQuery] int age

BindRequired is coming from BindRequiredAttribute class

2. Controller properties

3. Page handler parameters

4. Page model properties

## middleware
1. built-in

2. custom

      a. inline middleware with async
      b. middleware in class: convention and Factory-based middleware
      c. middleware extension and di/ioc: constructor or method

convention: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-7.0
factory-based: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/extensibility?view=aspnetcore-7.0

dependency injection (DI) software design pattern, which is a technique for achieving Inversion of Control (IoC) between classes and their dependencies.
dependency tree, dependency graph, or object graph.


## request/response
Pipeline vs Stream

HttpRequest exposed usages for both pipeline and stream:
HttpRequest.Body is a Stream, 
HttpResponse.Body is a Stream
HttpRequest.BodyReader is a PipeReader
HttpRequest.BodyWrite is a PipeWriter

PipeReader is to replace stream due to: no need for a string buffer 

## Request decompression

decompression providers: 

custom decompression provider: 
: IDecompressionProvider

## Response Cache

Cache-Control header: 

[ResponseCache] in controller

middleware

## url rewriting middleware
URL rewriting: modifying request URLs based on one or more predefined rules.

URL redirect vs URL rewrite ?

URL redirect: client side, extra round trip, 301: perm, 302: tmp

URL rewrite: server side, internal hop, viz-3d-service-k8s refactoring ?  
https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/fundamentals/url-rewriting/samples/6.x/SampleApp

prefer:
Apache mod_rewrite module on Apache Server
URL rewriting on Nginx

## Http Context
HttpContext encapsulates all information about an individual HTTP request and response. An HttpContext instance is initialized when an HTTP request is received. The HttpContext instance is accessible by middleware and app frameworks such as Web API controllers, Razor Pages, SignalR, gRPC, and more.

 controller: ControllerBase.HttpContext

 middleware:  public async Task InvokeAsync(HttpContext context)

## swagger
Swashbuckle.AspNetCore
The two main OpenAPI implementations for .NET are Swashbuckle and NSwag
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />

a Swagger generator that builds SwaggerDocument objects directly from your routes, controllers, and models. It's typically combined with the Swagger endpoint middleware to automatically expose Swagger JSON.


```shell
dotnet add server.csproj package Swashbuckle.AspNetCore -v 6.5.0
# generated swagger
# http://localhost:5256/swagger/v1/swagger.json
# http://localhost:5256/swagger/index.html

```