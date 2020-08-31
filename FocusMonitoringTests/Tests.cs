using System;
using System.IO;
using System.Linq;
using FocusAccess;
using FocusAccess.ResponseClasses;
using FocusMonitoring;
using Moq;
using NUnit.Framework;

namespace FocusMonitoringTests
{
    [TestFixture]
    public class Tests
    {
        private const string checkCode = "530278";
        private static readonly InnUrlArg checkInn = (InnUrlArg)(INN)"6663003127";
        
        [SetUp]
        public void SetUp()
        {
            using var writer  = new MonitoringSet("./OnMonitoring");
            writer.Targets.Add(new MonitoringTarget(ApiMethodEnum.analytics, checkInn));
            //writer.Write($"{DateTime.Now} {ApiMethodEnum.analytics} {checkInn}");
        }

        [Test]
        public void Should_UpdateFileAndCollection_When_PerformMonitoringCalled()
        {
            var api = new Mock<IApi3>();
            var diffApi = new Mock<IDifferentialApi>();
            diffApi.Setup(x => x.GetValue(ApiMethodEnum.analytics,checkInn)).Returns(checkCode);
            var monitorer = new Monitorer(api.Object,diffApi.Object);
            var result = monitorer.PerformMonitoring();
            
            Assert.AreEqual(result,checkCode);
            var file =  new MonitoringTarget(ApiMethodEnum.analytics, checkInn)
                .MakeFileName();
            Assert.IsTrue(File.ReadAllText($"./{file}").Contains(checkCode));
        }
        
        //[Test]
        public void Test()
        {
            InnUrlArg arg = (INN) "6663003127";
            var analytics = new Api(new FocusKey("3208d29d15c507395db770d0e65f3711e40374df"))
                .GetValue(ApiMethodEnum.analytics,arg);
            
            var api = new Mock<IApi3>();
            var diffApi = new Mock<DifferentialApi>();
            diffApi.Setup(x => x.GetValue(ApiMethodEnum.analytics,arg)).Returns("");
            //TestContext.CurrentContext.TestDirectory.Clone();
            
            var monitorer = new Monitorer(api.Object,diffApi.Object);
            monitorer.PerformMonitoring();
            (analytics as AnalyticsValue).Analytics.Q2001 = 123;//kpp 668601001

            var collectionChanged = false;
            var collection = new ChangesCollection();
            collection.CollectionChanged += (e, a) => collectionChanged = true;
            monitorer.PerformMonitoring();
            
            Assert.True(collectionChanged);
            var change = collection.First();
            Assert.AreEqual("Ul.Kpp",change.ChangedField);
            Assert.AreEqual(DateTime.Today.Date,change.Date);
            Assert.AreEqual(668601001,change.PastValue);
            Assert.AreEqual(123,change.NewValue);
        }
    }
}