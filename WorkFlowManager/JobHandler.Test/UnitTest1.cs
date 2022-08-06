using System;
using System.Linq;
using System.Threading;
using JobHandler.Executor;
using JobHandler.RabbitMq.Executor;
using JobHandler.RabbitMq.Sender;
using JobHandler.Sender;
using NUnit.Framework;
using RabbitMQ.Client;

namespace JobHandler.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1_1Job()
        {
            var helper = new JobHelper("group1");
            helper.Start();
            helper.Send(new Job { Subject = "Job 1" });
            Thread.Sleep(1000);
            helper.Stop();
            Assert.AreEqual(helper.ReceivedJobs.Count, 1);
            Assert.AreEqual(helper.ReceivedJobs.ToList()[0].Subject, "Job 1");
            Assert.LessOrEqual(helper.ReceivedJobs.ToList()[0].Lag, 100);
        }

        [Test]
        public void Test2_StopTest()
        {
            var helper = new JobHelper("group1");
            helper.Start();
            helper.Send(new Job { Subject = "Job 1" });
            Thread.Sleep(1000);
            helper.Stop();
            helper.Send(new Job { Subject = "Job 2" });
            Assert.AreEqual(helper.ReceivedJobs.Count, 1);
            Assert.AreEqual(helper.ReceivedJobs.ToList()[0].Subject, "Job 1");
            Assert.LessOrEqual(helper.ReceivedJobs.ToList()[0].Lag, 100);
        }

        [Test]
        public void Test3_MaxThreadsTest1()
        {
            var helper = new JobHelper("group1", 5);
            helper.Start();
            helper.Send(new Job { Subject = "Job 1" });
            helper.Send(new Job { Subject = "Job 2" });
            helper.Send(new Job { Subject = "Job 3" });
            helper.Send(new Job { Subject = "Job 4" });
            helper.Send(new Job { Subject = "Job 5" });
            Thread.Sleep(1500);
            helper.Stop();
            Assert.AreEqual(helper.ReceivedJobs.Count, 5);
        }

        [Test]
        public void Test3_MaxThreadsTest2()
        {
            var helper = new JobHelper("group1", 5);
            helper.Start();
            helper.Send(new Job { Subject = "Job 1" });
            helper.Send(new Job { Subject = "Job 2" });
            helper.Send(new Job { Subject = "Job 3" });
            helper.Send(new Job { Subject = "Job 4" });
            helper.Send(new Job { Subject = "Job 5" });
            helper.Send(new Job { Subject = "Job 6" });
            helper.Send(new Job { Subject = "Job 7" });
            Thread.Sleep(1500);
            helper.Stop();
            Assert.AreEqual(helper.ReceivedJobs.Count, 5);
        }

        [Test]
        public void Test4_InvalidMessage()
        {
            var helper = new JobHelper("group1", 5);
            helper.Start();
            helper.Send("Job 1");
            Thread.Sleep(1500);
            helper.Stop();
            Assert.AreEqual(helper.ReceivedJobs.Count, 0);

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var res = channel.BasicGet("group1_FailJobs", false);
            Assert.IsNotNull(res);
        }
    }
}