# GrpcPool

gRPC Client Pool Library for .Net

*Project Description*

gRPC is a Google remote procedure calling framework.

GrpcPool is base Google.Api.Gax.Grpc.

This sample is a library contains an gRPC client that you can use to connect to any gRPC. It is developed in C# language and works on all the following .Net platforms :

* .Net Framework (up to 4.5)
* not test below:
* .Net Compact Framework 3.5 & 3.9 (for Windows Embedded Compact 7 / 2013)
* .Net Micro Framework 4.2 & 4.3
* Mono (for Linux O.S.)
* .Net Core 1.0

*Bin package install*
```
Install-Package GrpcPool -Pre
```

For all information about gRPC, please visit official web site  http://www.grpc.io/.

*Building the source code*

The library is available for the following solution and project files :

* GrpcPool.sln : solution for Visual Studio 2015 that contains projects file for .Net Framework 4.5.2.

*SSL/TLS support*

The SSL/TLS feature, searching the test code please.

*Example*

Following an example of client to sync get pool :

```
IEnumerable<string> emptyScopes = Enumerable.Empty<string>();
ServiceEndpoint endpoint = new ServiceEndpoint(TestCredentials.DefaultHostOverride, Port);
var pool = new GrpcPool(emptyScopes);
var channel = pool.GetChannel(endpoint);
var client = new Greeter.GreeterClient(channel);
String user = "you";

var reply = client.SayHello(new HelloRequest { Name = user });
Console.WriteLine("Greeting: " + reply.Message);

channel.ShutdownAsync().Wait();
```

Following an example of client to sync get SSL pool :

```
IEnumerable<string> emptyScopes = Enumerable.Empty<string>();
ServiceEndpoint endpoint = new ServiceEndpoint(TestCredentials.DefaultHostOverride, SslPort);
var pool = new GrpcPool(emptyScopes);
var channel = pool.GetChannel(endpoint, clientCredentials);
var client = new Greeter.GreeterClient(channel);
String user = "you";

var reply = client.SayHello(new HelloRequest { Name = user });
Console.WriteLine("Greeting: " + reply.Message);

channel.ShutdownAsync().Wait();
```

Following an example of client to async get pool :

```
await Task.Run(async () =>
{
    IEnumerable<string> emptyScopes = Enumerable.Empty<string>();
    ServiceEndpoint endpoint = new ServiceEndpoint(TestCredentials.DefaultHostOverride, Port);
    var pool = new GrpcPool(emptyScopes);
    var channel = await pool.GetChannelAsync(endpoint);
    var client = new Greeter.GreeterClient(channel);
    String user = "you";

    var reply = await client.SayHelloAsync(new HelloRequest { Name = user });
    Console.WriteLine("Greeting: " + reply.Message);

    await channel.ShutdownAsync();
});
```

Following an example of client to async get SSL pool :

```
await Task.Run(async () =>
{
    IEnumerable<string> emptyScopes = Enumerable.Empty<string>();
    ServiceEndpoint endpoint = new ServiceEndpoint(TestCredentials.DefaultHostOverride, SslPort);
    var pool = new GrpcPool(emptyScopes);
    var channel = await pool.GetChannelAsync(endpoint, clientCredentials);
    var client = new Greeter.GreeterClient(channel);
    String user = "you";

    var reply = await client.SayHelloAsync(new HelloRequest { Name = user });
    Console.WriteLine("Greeting: " + reply.Message);

    await channel.ShutdownAsync();
});
```
