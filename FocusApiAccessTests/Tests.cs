using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using FocusAccess;
using FocusAccess.ResponseClasses;
using MockHttpServer;
using Mono.CSharp;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Enum = System.Enum;
using Is = NUnit.DeepObjectCompare.Is;
using MoreLinq;

namespace FocusApiAccessTests
{
    [TestFixture]
    public class Tests
    {
        private Api Api;
        private DifferentialApi DiffApi { get; set; }

        private const string test = "[{\"inn \":\"7708037057\",\"ogrn\":\"1037739696466\",\"focusHref\":\"https:\\/\\/focus.kontur.ru\\/entity?query=1037739696466\",\"UL\":{\"kpp\":\"773001001\",\"legalName\":{\"short\":\"ГТК России\",\"full\":\"Государственный таможенный комитет Российской Федерации\",\"date\":\"2003-02-18\"},\"legalAddress\":{\"parsedAddressRF\":{\"zipCode\":\"121087\",\"kladrCode\":\"770000000002027\",\"regionCode\":\"77\",\"regionName\":{\"topoShortName\":\"г\",\"topoFullName\":\"город\",\"topoValue\":\"Москва\"},\"street\":{\"topoShortName\":\"ул\",\"topoFullName\":\"улица\",\"topoValue\":\"Новозаводская\"},\"house\":{\"topoShortName\":\"дом\",\"topoFullName\":\"дом\",\"topoValue\":\"11\"},\"bulk\":{\"topoValue\":\"5\"},\"houseRaw\":\"11\",\"bulkRaw\":\"5\"},\"date\":\"2003-02-18\",\"firstDate\":\"2003-02-18\"},\"status\":{\"statusString\":\"Прекращение деятельности юридического лица путем реорганизации в форме преобразования\",\"dissolved\":true,\"date\":\"2004-09-09\"},\"registrationDate\":\"1994-10-25\",\"dissolutionDate\":\"2004-09-09\",\"history\":{}},\"briefReport\":{\"summary\":{\"redStatements\":true}},\"contactPhones\":{}}]";
        private const string keyString = "3208d29d15c507395db770d0e65f3711e40374df";
        private static readonly string[] testableInns = {
            "6663003127", "561100409545",
            "7708503727", "666200351548",
            "7736050003", "366512608416",
            "7452027843", "773173084809",
            "6658021579", "771409116994",
            "7725604637", "503115929542",
            "4401006984", "773400211252",
            "3016003718", "771902452360",
            "5053051872", "702100195003"
        };

        [SetUp]
        public void TestPrepare()
        {
            Settings.CachePath = "C:\\Users\\shetnikov\\Desktop\\";
            Settings.ApiUrl = "http://localhost:"+61666+"/";
            var key = new FocusKey(keyString);
            Api = new Api(key); //FocusKeyManager.StartAccess("fdc7d0cd30185a63331724bc69d6dc625476048b").GetApi();
            DiffApi = new DifferentialApi(key);
        }


        /*

        [Test]
        public void Test1()
        {
            using (new MockServer(61666, "/req?key=3208d29d15c507395db770d0e65f3711e40374df&inn=7708037057",
                (req, rsp, prm) => test))
            {
                var result = Api.Req.MakeRequest((INN) "7708037057").Ogrn;
                Assert.AreEqual(result,"1037739696466");
            }
        }
        
        [Test]
        public void Test3()
        {
            var data = Api.Req.MakeRequest((INN)"7708037057").BriefReport.Summary.RedStatements;
            Assert.IsTrue(data);
            
        }
        
        [Test]
        public void Test4()
        {
            var data = Api.Req.MakeRequest((INN)"7722385338")?.Ul.ManagementCompanies[0].Inn;
            Assert.True(data == "7705059380");//TODO unploho
        }
        [Test]
        public void Test5()
        {
            /*var server = WireMockServer.Start();
            server.Given(Request.Create().WithPath("/req").UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "text/plain")
                    .WithBody(test));#1#
            var data = Api.Req.MakeRequest((INN) "6663003127")?.Ul.Branches[0].ParsedAddressRf.House.TopoValue;
            //Thread.Sleep(12);
            Assert.True(data == "3");
        }
        
        [Test]
        public void Test6()
        {

            var data = Api.CompanyAffiliatesReq.MakeRequest((INN) "7414006722")[3].BriefReport.Summary.RedStatements;
                Assert.IsTrue(data);
        }

        [Test]
        public void Test7()
        {
            Settings.ApiUrl = "http://localhost:"+61666;
            Api.Req.MakeRequest((INN) "6663003127");
            using (new MockServer(61666, "/req?key=3208d29d15c507395db770d0e65f3711e40374df&inn=7708037057",
                (req, rsp, prm) => test))
            {
                
                var data = Api.Req.MakeRequest((INN) "6663003127")?.Ul.Branches[0].ParsedAddressRf.House.TopoValue;
                Assert.AreEqual(data,"3");
            }
        }*/

        [TestCaseSource(nameof(testableInns))]
        public void Test(string inn) 
        {
            const string path = "C:\\Users\\shetnikov\\Documents\\GitHub\\FocusScoring\\FocusApiAccessTests\\JSONServerResponces";
            foreach (var method in Enum.GetValues(typeof(ApiMethodEnum)).Cast<ApiMethodEnum>().Where(m=>m.GoodDbg()))
            {
                var file = $"{path}\\{inn}.{method.Alias()}";
                if(!File.Exists(file)) continue;
                var json = File.ReadAllText(file);
                
                using (new MockServer(61666, $"/{method.Url()}?key={keyString}&inn={inn}",
                    (req, rsp, prm) => json))  
                {
                    var actual = Api.GetValue(method, (InnUrlArg) (INN) inn);
                    var value = JsonConvert.DeserializeObject(json, method.ValueType(),Converter.Settings);
                    var expected = ((IList) value).Cast<IParameterValue>().First();
                    Assert.That(actual,Is.DeepEqualTo(expected));
                }
            }
        }
                
            //TODO finish both of them

        /*private static List<MockHttpHandler> Handlers()
        {
            Enum.GetValues(typeof(ApiMethodEnum))
                .Cast<ApiMethodEnum>()
                .Where(m=>m.GoodDbg())
                .Cartesian(testableInns,
                    (method, inn) =>
                    {
                        new MockHttpHandler("/req?key={keyString}&inn={inn}",(req, rsp, prm) => expected))
                    })
        };*/
        
        [TestCaseSource(nameof(testableInns))]
        public void DifferentialTest(string inn)
        {
            const ApiMethodEnum method = ApiMethodEnum.req;
            const string path = "C:\\Users\\shetnikov\\Documents\\GitHub\\FocusScoring\\FocusApiAccessTests\\JSONServerResponces";
            var file = $"{path}\\{inn}.{method.Alias()}";
            if(!File.Exists(file)) throw new Exception("No memes here! It's a Cristian server.");
            var origJson = File.ReadAllText(file);
            var apiUsed = false;
            var origObj = JsonConvert.DeserializeObject<IList<ReqValue>>(origJson);
            try
            {
                var newJson = origJson.Replace("\"statusString\":\"","\"statusString\":\"Prefix is here ");
                var isFl = ((INN) origObj[0].Inn).IsFL();

                using (new MockServer(61666, $"/{method.Url()}?key={keyString}&inn={inn}",
                    (req, rsp, prm) =>
                    {
                        var result = apiUsed ? origJson : newJson;
                        apiUsed = false;   
                        return result;
                    }))
                {
                    var actual = DiffApi.GetValue(method, (InnUrlArg) (INN) inn);
                    //var value = JsonConvert.DeserializeObject(origJson, method.ValueType(),Converter.Settings);
                    //var expected = ((IList) value).Cast<IParameterValue>().First();
                    Assert.AreEqual("Where were Gondor?",actual );
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                File.WriteAllText(file,origJson);
            }
            
        }

        private string ChangeJson(string origJson)
        {
            //"statusString":"
            /*
            var origObj = JsonConvert.DeserializeObject<IList<ReqValue>>(origJson);
            if ()
                origObj[0].Ip.Oktmo = "Embrace the futility of your mum!";
            else
                origObj[0].Ul.Oktmo = "Embrace the futility of your mum!";
            var newJson = JsonConvert.SerializeObject(origObj);*/
            origJson.Replace("\"statusString\":\"","\"statusString\":\"Prefix is here ");
            return null;
        }
        
        //[TestCaseSource(nameof(testableInns))]
        /*public void Should_KeepInformationCorrect_When_AnyInnRequested(string inn)
        {
            //var responses = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.FullName ?? throw new Exception(), @"JSONServerResponces");
            //var a = Directory.EnumerateFiles(responses).Where(x => x.StartsWith(inn)).ToArray();
            const string path = "C:\\Users\\shetnikov\\Documents\\GitHub\\FocusScoring\\FocusApiAccessTests\\JSONServerResponces";
            var ass = Assembly.GetAssembly(typeof(Api3));
            var deb = ass.GetType("FocusApiAccess.Methods.IApiMultiValueMethod`2");
            foreach (var prop in typeof(Api3).GetProperties()
                .Where(x=> ! x.PropertyType.IsAssignableFrom( ass.GetType("FocusApiAccess.Methods.IApiMultiValueMethod'2")) 
                           && x.PropertyType.GenericTypeArguments[1] == typeof(InnUrlArg)))
            {
                var alias = Assembly.GetAssembly(typeof(Api3))
                    .GetType("FocusApiAccess.Methods.IApiMethod`2")
                    .MakeGenericType(prop.PropertyType.GenericTypeArguments)
                    .GetMethod("MakeAlias")?  //Embrace the clusterfuck of reflective testing!
                    .Invoke(prop.GetValue(Api), new object[0]);
                var expected = File.ReadAllText($"{path}\\{inn}.{alias}");
                using (new MockServer(61666, $"/req?key={keyString}&inn={inn}",
                    (req, rsp, prm) => expected))
                {
                    var actual = JsonConvert.SerializeObject(Api.Req.MakeRequest((INN) inn));
                    Assert.AreEqual(expected,actual);
                }
            }
        }*/

        //[Test]
        public void Load()
        {

            var s = new List<Exception>();
            foreach (var inn in testableInns)
            {
                /*var res = typeof(Api3).GetProperties()
                    .Where(x => x.PropertyType.GenericTypeArguments[1] == typeof(InnUrlArg))
                    .Select(x =>
                    {
                        try
                        {
                            (x.PropertyType.GetMethod("MakeRequest") ?? 
                             x.PropertyType.BaseType?.GetMethod("MakeRequest") ?? 
                             throw new ArgumentException("adf"))
                                .Invoke(x.GetValue(Api), new object[] {(InnUrlArg) (INN) inn, false});
                        }
                        catch(Exception e)
                        {
                            return x.Name + " NO";
                        }
                        return x.Name + " yes";
                    })
                    .ToArray();
                var resSkip = typeof(Api3).GetProperties()
                    .Where(x => x.PropertyType.GenericTypeArguments[1] == typeof(SkipableInnUrlArg))
                    .Select(x =>
                    {
                        try
                        {
                            x.PropertyType.GetMethod("MakeRequest")
                                .Invoke(x.GetValue(Api), new object[] {new SkipableInnUrlArg((INN) inn,0) , false});
                        }
                        catch(Exception e)
                        {
                            return e;
                        }
                        return null;
                    })
                    .ToArray(); */

                /*try
                {
                    Thread.Sleep(100);
                    Api.Analytics.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.Buh.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.Contacts.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.Licences.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.Fsa.MakeRequest(new SkipableInnUrlArg(inn, 0));
                    Thread.Sleep(100);
                    Api.Fssp.MakeRequest(new SkipableInnUrlArg(inn, 0));
                    Thread.Sleep(100);
                    Api.Req.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.Sites.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.Taxes.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.Taxes.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.BeneficialOwners.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.EgrDetails.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.Trademarks.MakeRequest(new SkipableInnUrlArg(inn, 0));
                    Thread.Sleep(100);
                    Api.BankGuarantees.MakeRequest(new SkipableInnUrlArg(inn, 0));
                    Thread.Sleep(100);
                    Api.GovPurchasesOfCustomer.MakeRequest(new SkipableInnUrlArg(inn, 0));
                    Thread.Sleep(100);
                    Api.GovPurchasesOfParticipant.MakeRequest(new SkipableInnUrlArg(inn, 0));
                    Thread.Sleep(100);
                    Api.CompanyAffiliatesAnalytics.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.CompanyAffiliatesReq.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.CompanyAffiliatesEgrDetails.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                    Api.FnsBlockedBankAccounts.MakeRequest((INN) inn);
                    Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    s.Add(e);
                }*/
            }
            
            Assert.True(true);
        }

        //[Test]
        public void Should_CorrectlyDeserializeEnum()
        {
            using (new MockServer(61666, "/req?key=3208d29d15c507395db770d0e65f3711e40374df&inn=7708037057",
                (req, rsp, prm) => test))
            {
                //Assert.That(Api.Analytics.MakeRequest((INN) "3016003718").Analytics.E7014 == E7014.КонкурсноеПроизводство);
            }
        }
        
        /*
        [Test]
        public void Should_SaveCacheInDifferentFiles_When_MakeMultiRequestCalled()
        {
            
        }*/
    }
}