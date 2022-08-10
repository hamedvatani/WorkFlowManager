using NUnit.Framework;
using WorkFlowManager.Test.ShoppingCard;

namespace WorkFlowManager.Test
{
    public class Tests
    {
        private WorkFlowHelper _helper;

        [SetUp]
        public void Setup()
        {
            _helper = new WorkFlowHelper();
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual("Hello",_helper.SayHello());
        }
    }
}