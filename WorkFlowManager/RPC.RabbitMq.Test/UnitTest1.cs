using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace RPC.RabbitMq.Test
{
    public class Tests
    {
        private RpcServer _server = null!;
        private RpcClient _client = null!;

        [SetUp]
        public void Setup()
        {
            RpcConfiguration configuration = new RpcConfiguration
            {
                 Timeout = 5000
            };
            _server = new RpcServer(configuration);
            _client = new RpcClient(configuration);
        }

        [Test]
        public void Test1_NormalCall()
        {
            _server.Start(dto =>
            {
                if (dto.FunctionName == "Sum")
                {
                    int a1, a2;
                    if (dto.Parameters.ContainsKey("A1"))
                    {
                        a1 = int.Parse(dto.Parameters["A1"]);
                        if (dto.Parameters.ContainsKey("A2"))
                        {
                            a2 = int.Parse(dto.Parameters["A2"]);
                            return new RpcResultDto(true, false, "",
                                new KeyValuePair<string, string>("Sum", (a1 + a2).ToString()));
                        }
                    }
                }

                return new RpcResultDto(false, false, "Error");
            });
            _client.Start();

            var response = _client.Call(new RpcFunctionDto("Sum", new KeyValuePair<string, string>("A1", "10"),
                new KeyValuePair<string, string>("A2", "20")));

            Assert.AreEqual(true, response.IsSuccess);
            Assert.AreEqual(false, response.IsTimeout);
            Assert.AreEqual("", response.Message);
            Assert.AreEqual(1, response.Parameters.Count);
            Assert.AreEqual(true, response.Parameters.ContainsKey("Sum"));
            Assert.AreEqual("30", response.Parameters["Sum"]);
        }

        [Test]
        public void Test2_Timeout()
        {
            _server.Start(dto =>
            {
                if (dto.FunctionName == "Sum")
                {
                    int a1, a2;
                    if (dto.Parameters.ContainsKey("A1"))
                    {
                        a1 = int.Parse(dto.Parameters["A1"]);
                        if (dto.Parameters.ContainsKey("A2"))
                        {
                            a2 = int.Parse(dto.Parameters["A2"]);
                            Thread.Sleep(10000);
                            return new RpcResultDto(true, false, "",
                                new KeyValuePair<string, string>("Sum", (a1 + a2).ToString()));
                        }
                    }
                }

                return new RpcResultDto(false, false, "Error");
            });
            _client.Start();

            var response = _client.Call(new RpcFunctionDto("Sum", new KeyValuePair<string, string>("A1", "10"),
                new KeyValuePair<string, string>("A2", "20")));

            Assert.AreEqual(false, response.IsSuccess);
            Assert.AreEqual(true, response.IsTimeout);
            Assert.AreEqual("Timeout", response.Message);
        }
    }
}