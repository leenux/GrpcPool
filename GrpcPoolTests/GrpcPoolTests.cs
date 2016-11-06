using Microsoft.VisualStudio.TestTools.UnitTesting;
using GrpcPool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helloworld;
using Grpc.Core;
using Google.Api.Gax.Grpc;

namespace GrpcPool.Tests
{
    class GreeterImpl : Greeter.GreeterBase
    {
        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = "leenux=Hello " + request.Name });
        }
    }

    [TestClass()]
    public class GrpcPoolTests
    {
        private const int Port = 50051;
        private const int SslPort = 50052;
        private Server _server;
        private string rootCert => File.ReadAllText(TestCredentials.ClientCertAuthorityPath);
        KeyCertificatePair keyCertPair => new KeyCertificatePair(
            File.ReadAllText(TestCredentials.ServerCertChainPath),
            File.ReadAllText(TestCredentials.ServerPrivateKeyPath));

        SslServerCredentials serverCredentials => new SslServerCredentials(new[] { keyCertPair }, rootCert, true);
        SslCredentials clientCredentials => new SslCredentials(rootCert, keyCertPair);

        [TestInitialize]
        public void Setup()
        {
            _server = new Server
            {
                Services = { Greeter.BindService(new GreeterImpl()) },
                Ports = { new ServerPort(TestCredentials.DefaultHostOverride, Port, ServerCredentials.Insecure),
                    new ServerPort(TestCredentials.DefaultHostOverride, SslPort, serverCredentials) }
            };
            _server.Start();
        }
        [TestMethod()]
        public void GetChannelTest()
        {
            IEnumerable<string> emptyScopes = Enumerable.Empty<string>();
            ServiceEndpoint endpoint = new ServiceEndpoint(TestCredentials.DefaultHostOverride, Port);
            var pool = new GrpcPool(emptyScopes);
            var channel = pool.GetChannel(endpoint);
            var client = new Greeter.GreeterClient(channel);
            String user = "you";

            var reply = client.SayHello(new HelloRequest { Name = user });
            Console.WriteLine("Greeting: " + reply.Message);

            channel.ShutdownAsync().Wait();
            Assert.AreEqual(reply.Message,"leenux=Hello you");
        }

        [TestCleanup]
        public void Finish()
        {
            _server.ShutdownAsync().Wait();
        }

        [TestMethod()]
        public void GetSslChannelTest()
        {
            IEnumerable<string> emptyScopes = Enumerable.Empty<string>();
            ServiceEndpoint endpoint = new ServiceEndpoint(TestCredentials.DefaultHostOverride, SslPort);
            var pool = new GrpcPool(emptyScopes);
            var channel = pool.GetChannel(endpoint, clientCredentials);
            var client = new Greeter.GreeterClient(channel);
            String user = "you";

            var reply = client.SayHello(new HelloRequest { Name = user });
            Console.WriteLine("Greeting: " + reply.Message);

            channel.ShutdownAsync().Wait();
            Assert.AreEqual(reply.Message, "leenux=Hello you");
        }

        [TestMethod()]
        public async Task GetChannelAsyncTest()
        {
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
                Assert.AreEqual(reply.Message, "leenux=Hello you");
            });
        }

        [TestMethod()]
        public async Task GetSslChannelAsyncTest()
        {
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
                Assert.AreEqual(reply.Message, "leenux=Hello you");
            });
        }
    }
}