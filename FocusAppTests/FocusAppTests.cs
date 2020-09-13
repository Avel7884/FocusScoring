using System;
using System.IO;
using System.Linq;
using FocusAccess;
using FocusApp;
using FocusScoring;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace FocusAppTests
{
    [TestFixture]
    public class FocusAppTests
    {
        private IDataManager manager;
        private IApi3 Api;
        private FocusKey Key;
        private IFocusDataBase<INN> CurrentDataBase;

        private static readonly INN[] testInns = @"6663003127 561100409545 7708503727 666200351548 7736050003 366512608416 7452027843 773173084809 6658021579 771409116994 7725604637 503115929542 4401006984 773400211252 3016003718 771902452360 5053051872 702100195003"
            .Split( ' ').Select(x=>(INN)x).ToArray();
        private static readonly SubjectParameter[] baseParams = new []{SubjectParameter.Address, SubjectParameter.Inn};
        
        private const SubjectParameter newParam = SubjectParameter.FIO;
        
        [SetUp]
        public void SetUp()
        {
            Key = new FocusKey("3208d29d15c507395db770d0e65f3711e40374df");
            Api = new Api(Key);
            Directory.CreateDirectory(Settings.CachePath + Settings.AppCacheFolder);
            var scorer = ScorerFactory.CreateEmptyINNScorer();
            manager = new DataManager(new EntryFactory(Api, scorer));
        }


        [Test, Order(1)]
        public void Should_MatchInfoAndData_When_CreateNewLists()
        {
            TestListsCreation(testInns.Take(9).ToArray());
            TestListsCreation(testInns.Skip(9).ToArray());
        }

        private void TestListsCreation(INN[] inns)
        {
            var info = new DataInfo("test",baseParams);
            CurrentDataBase = manager.CreateNew(info,inns);
            Assert.AreEqual(inns,CurrentDataBase.Select(x => x.Data[1]));
            Assert.AreEqual(info,CurrentDataBase.Info);
        }

        [Test, Order(2)]
        public void Should_MatchInfoAndData_When_OpenNewList()
        {
            var prevData = CurrentDataBase.Select(x => x.Data[1]);
            var prevInfo = CurrentDataBase.Info;
            CurrentDataBase = manager.OpenNew(manager.Infos[0]);
            Assert.AreEqual(prevData,CurrentDataBase.Select(x => x.Data[1]));
            Assert.AreEqual(prevInfo,CurrentDataBase.Info);
        }

        [Test, Order(3)]
        public void Should_MatchCountAndData_When_AddNewEntry()
        {
            var inn = testInns.Last();
            var lastCount = CurrentDataBase.Count; 
            CurrentDataBase.Write(inn);
            Assert.IsTrue(CurrentDataBase.Any(x=>x.Data[1] == inn.ToString()));
            Assert.AreEqual(lastCount+1,CurrentDataBase.Info.Length);
        }
        
        [Test, Order(4)]
        public void Should_MatchParametersAndData_When_AddNewColumn()
        {
            var lastParamLength = CurrentDataBase.Info.Parameters.Count;
            Assert.That(()=>!CurrentDataBase.Info.Parameters.Contains(newParam));
            CurrentDataBase.AddColumns(new[]{newParam});
            Assert.That(()=>CurrentDataBase.Info.Parameters.Contains(newParam));
            Assert.AreEqual(lastParamLength+1,CurrentDataBase.Info.Parameters.Count);
        }
        
        [Test, Order(5)]
        public void Should_MatchCountAndData_When_DeleteEntry()
        {
            var inn = CurrentDataBase.Last().Subject;
            var lastCount = CurrentDataBase.Count;
            CurrentDataBase.Delete(inn);
            Assert.IsFalse(CurrentDataBase.Any(x=>x.Data[0] == inn.ToString()));
            Assert.AreEqual(lastCount-1,CurrentDataBase.Info.Length);
        }
        
        [Test, Order(6)]
        public void Should_MatchParametersAndData_When_DeleteColumn()
        {
            var paramIndex = CurrentDataBase.Info.Parameters.ToList().IndexOf(newParam);
            var lastParamLength = CurrentDataBase.Info.Parameters.Count;
            Assert.That(()=>CurrentDataBase.Info.Parameters.Contains(newParam));
            CurrentDataBase.RemoveColumn(paramIndex);
            Assert.That(()=>!CurrentDataBase.Info.Parameters.Contains(newParam));
            Assert.AreEqual(lastParamLength-1,CurrentDataBase.Info.Parameters.Count);
        }

        [Test, Order(7)]
        public void Should_ReorderColumnHeader()
        {
            var firstColumnInfo = CurrentDataBase.Info.Parameters[0];
            CurrentDataBase.ReorderColumns(0,1);
            Assert.AreEqual(firstColumnInfo,CurrentDataBase.Info.Parameters[1]);
        }

        [Test, Order(8)]    
        public void Should_ReorderColumnData()
        {
            var firstColumnData = CurrentDataBase.Select(x=>x.Data[0]);
            CurrentDataBase.ReorderColumns(0,1);
            Assert.AreEqual(firstColumnData,CurrentDataBase.Select(x=>x.Data[1]));
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete(Settings.CachePath + Settings.AppCachesIndexFileName);
            foreach (var file in Directory.EnumerateFiles(Settings.CachePath + Settings.AppCacheFolder))
                File.Delete(file);
        }
    }
}