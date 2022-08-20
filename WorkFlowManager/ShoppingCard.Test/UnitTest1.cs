using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using WorkFlowManager.Client;

namespace ShoppingCard.Test
{
    public class Tests
    {
        private ShoppingCardBiz _biz;

        [SetUp]
        public void Setup()
        {
            var config = new ClientConfiguration
            {
                ApiAddress = "localhost",
                ApiPort = 42578
            };
            _biz = new ShoppingCardBiz(new Client(config, new ApiClient(config), new RpcClient(config)));
            _biz.CreateWorkFlow();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}