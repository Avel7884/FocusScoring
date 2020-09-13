using System;
using System.IO;
using System.Linq;
using FocusAccess;
using FocusAccess.ResponseClasses;
using FocusMonitoring;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FocusMonitoringTests
{
    [TestFixture]
    public class Tests
    {
        private const string checkCode = "530278";
        private static readonly InnUrlArg checkInn = (INN)"6663003127";
        private MonitoringFactory factory = new MonitoringFactory();
        
        [SetUp]
        public void SetUp()
        {
            using var writer = factory.OpenSet();
            writer.Targets.Add(new MonitoringTarget(ApiMethodEnum.analytics, checkInn));
            //writer.Write($"{DateTime.Now} {ApiMethodEnum.analytics} {checkInn}");
        }

        [Test]
        public void Should_UpdateFileAndCollection_When_PerformMonitoringCalled()
        {
            var api = new Mock<IApi3>();
            var diffApi = new Mock<IDifferentialApi>();
            diffApi.Setup(x => x.GetValue(ApiMethodEnum.analytics,
                    It.Is<IQuery>(x => x.Values.Length == 1 && x.Values[0] == checkInn.Values[0])))
                .Returns(checkCode);
            var monitorer = new Monitorer(api.Object,diffApi.Object,factory);
            monitorer.PerformMonitoring();
            var result = JsonConvert.DeserializeObject<MonitoringResult>(File.ReadAllText(".\\ShortLog"));
            
            Assert.AreEqual(result.Changes,checkCode);
            var file =  new MonitoringTarget(ApiMethodEnum.analytics, checkInn)
                .MakeFileName();
            Assert.IsTrue(File.ReadAllText($"./{file}").Contains(checkCode));
        }
        
        //[Test]
        /*public void Test()
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
        }*/
    }
}