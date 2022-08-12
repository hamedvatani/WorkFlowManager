using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WorkFlowManager.Core.Contract;
using WorkFlowManager.Core.Data;
using WorkFlowManager.Core.Repository;
using WorkFlowManager.Test.ShoppingCard;

namespace WorkFlowManager.Test
{
    public class Tests
    {
        private WorkFlowHelper? _helper;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddTransient<IManager, Manager>();
            services.AddTransient<WorkFlowHelper>();
            services.AddTransient<IRepository, TestRepository>();
            var serviceProvider = services.BuildServiceProvider();
            _helper = serviceProvider.GetService<WorkFlowHelper>();
        }

        [Test]
        public void Test1()
        {
        }
    }
}