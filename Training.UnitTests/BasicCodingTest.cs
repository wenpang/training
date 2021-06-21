using Training.UnitTests.Models;
using Training.UnitTests.TestCases;
using Training.Models;
using Training.Controllers;
using Training.Converters;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Training.UnitTests
{
    public class BasicCodingTest
    {
        [Test]
        public void MultiThread()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Task task1 = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
            });

            Task task2 = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
            });

            Task task3 = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(3000);
            });

            Task.WaitAll(task1, task2, task3);

            sw.Stop();

            TestContext.WriteLine($"執行時間:{sw.ElapsedMilliseconds} ms");
        }

        #region - Serialize & Deserialize -

        [Test]
        public void ObjectToJson()
        {
            List<DemoViewModel> productList = new List<DemoViewModel>
            {
                new DemoViewModel { ProductID = "00000000001", ProductNM = "機票", Quantity = 100, IsDisable = false },
                new DemoViewModel { ProductID = "00000000002", ProductNM = "訂房", Quantity = 100, IsDisable = false },
                new DemoViewModel { ProductID = "00000000003", ProductNM = "自由行", Quantity = 100, IsDisable = false }
            };

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                //Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs), //允許基本拉丁英文及中日韓文字維持原字元
                PropertyNamingPolicy = null,//預設是 JsonNamingPolicy.CamelCase，強制頭文字轉小寫; 維持原樣，設為null
                //WriteIndented = true // 自動排版
            };

            string productJsonStr = JsonSerializer.Serialize(productList, options);

            TestContext.WriteLine(productJsonStr);
            Assert.IsNotNull(productJsonStr);
        }

        [Test]
        public void JsonToObject()
        {
            string productJsonStr = "[{\"ProductID\":\"00000000001\",\"ProductNM\":\"機票\",\"Quantity\":100,\"IsDisable\":false},{\"ProductID\":\"00000000002\",\"ProductNM\":\"訂房\",\"Quantity\":100,\"IsDisable\":false},{\"ProductID\":\"00000000003\",\"ProductNM\":\"自由行\",\"Quantity\":100,\"IsDisable\":false}]";
            List<DemoViewModel> productObj = JsonSerializer.Deserialize<List<DemoViewModel>>(productJsonStr);

            Assert.IsNotNull(productObj);
        }

        #endregion

        #region - LINQ - 

        [TestCase(Category = "LINQ")]
        public void Linq_Where()
        {
            List<string> regionList = new List<string> { "Japan", "France", "Germany", "Greece", "Hong Kong", "India", "Italy", "Korea" };

            //lambda
            IEnumerable<string> queryLambda = regionList.Where(fruit => fruit.Length < 6);

            //link
            IEnumerable<string> query = from region in regionList
                                        where region.StartsWith('G')
                                        select region;

            foreach (string region in query)
            {
                TestContext.WriteLine(region);
            }

            Assert.Pass();
        }

        [TestCase(Category = "LINQ")]
        public void Linq_Find()
        {
            List<string> regionList = new List<string> { "Japan", "France", "Germany", "Greece", "Hong Kong", "India", "Italy", "Korea" };

            string queryFind = regionList.Find(r => r.EndsWith('y'));
            string queryFindLast = regionList.FindLast(r => r.EndsWith('y'));
            List<string> queryFindAll = regionList.FindAll(r => r.EndsWith('y'));

            TestContext.Out.WriteLine($"[Find] output: \n{queryFind}\n");
            TestContext.Out.WriteLine($"[FindLast] output: \n{queryFindLast}\n");

            TestContext.WriteLine("[FindAll] output:");
            foreach (string fruit in queryFindAll)
            {
                TestContext.WriteLine(fruit);
            }

            Assert.Pass();
        }

        [TestCase(Category = "LINQ")]
        public void Linq_Select()
        {
            string[] regionArr = { "Japan", "France", "Germany", "Greece", "Hong Kong", "India", "Italy", "Korea" };

            var query = regionArr.Select((region, index) => new { index, Name = region }); //匿名類型 (Anonymous Type)
            var querySelect = regionArr.Select((region, index) => new TicketModel { OrderId = $"P{index + 1}", Destination = region });

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            TestContext.WriteLine("匿名類型: \n" + JsonSerializer.Serialize(query, options));
            TestContext.WriteLine("實體類型: \n" + JsonSerializer.Serialize(querySelect, options));

            Assert.Pass();
        }

        [TestCase(Category = "LINQ")]
        public void Linq_ToArray()
        {
            List<TicketModel> ticketModelList = new List<TicketModel>
            {
                new TicketModel { Destination="Japan", PeopleNum=2},
                new TicketModel { Destination="France", PeopleNum=2},
                new TicketModel { Destination="Greece", PeopleNum=2},
                new TicketModel { Destination="Korea", PeopleNum=2},
                new TicketModel { Destination="Germany", PeopleNum=2},
            };

            string[] regionArr = ticketModelList.Select(t => t.Destination).ToArray();

            TestContext.WriteLine(JsonSerializer.Serialize(regionArr));

            Assert.Pass();
        }

        [TestCase(Category = "LINQ")]
        public void Linq_ToList()
        {
            string[] regionArr = { "Japan", "France", "Germany", "Greece", "Hong Kong", "India", "Italy", "Korea" };

            List<string> regionList = regionArr.ToList();

            TestContext.WriteLine(JsonSerializer.Serialize(regionList));

            Assert.Pass();
        }

        [TestCase(Category = "LINQ")]
        public void Linq_ToDictionary()
        {
            List<TicketModel> ticketModelList = new List<TicketModel>
            {
                new TicketModel { Destination="Japan", TotalPrice=1000},
                new TicketModel { Destination="France", TotalPrice=2000},
                new TicketModel { Destination="Greece", TotalPrice=3000},
                new TicketModel { Destination="Korea", TotalPrice=4000},
                new TicketModel { Destination="Germany", TotalPrice=5000},
            };

            Dictionary<string, decimal> ticketDic = ticketModelList.ToDictionary(key => key.Destination, value => value.TotalPrice);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            TestContext.WriteLine(JsonSerializer.Serialize(ticketDic, options));
            Assert.IsNotNull(ticketDic);
        }

        #endregion

        #region - Colloctions Convert -

        [TestCase(Category = "ColloctionsConvert", TestName = "ColloctionsConvert_ArrayToList")]
        public void ArrayToList()
        {
            string[] productArr = new string[] { "機票", "訂房", "自由行", "郵輪", "票券" };

            List<string> productList = productArr.ToList();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            TestContext.WriteLine(JsonSerializer.Serialize(productList, options));
            Assert.IsNotNull(productList);
        }

        [TestCase(Category = "ColloctionsConvert", TestName = "ColloctionsConvert_ArrayToDictionary")]
        public void ArrayToDictionary()
        {
            string[] productArr = new string[] { "機票", "訂房", "自由行", "郵輪", "票券" };
            Dictionary<int, string> productDic = productArr.ToDictionary(key => Array.IndexOf(productArr, key), value => value);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            options.Converters.Add(new JsonNonStringKeyDictionaryConverterFactory());

            TestContext.WriteLine(JsonSerializer.Serialize(productDic, options));
            Assert.IsNotNull(productDic);
        }

        [TestCase(Category = "ColloctionsConvert", TestName = "ColloctionsConvert_ArrayToSet")]
        public void ArrayToSet()
        {
            string[] productArr = new string[] { "機票", "訂房", "自由行", "機票", "訂房", "自由行" };

            HashSet<string> productSet = productArr.ToHashSet();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            TestContext.WriteLine(JsonSerializer.Serialize(productSet, options));
            Assert.IsNotNull(productSet);
        }

        [TestCase(Category = "ColloctionsConvert", TestName = "ColloctionsConvert_ListToArray")]
        public void ListToArray()
        {
            List<DemoViewModel> productList = new List<DemoViewModel>
            {
                new DemoViewModel { ProductID = "00000000001", ProductNM = "機票", Quantity = 100, IsDisable = false },
                new DemoViewModel { ProductID = "00000000002", ProductNM = "訂房", Quantity = 100, IsDisable = false },
                new DemoViewModel { ProductID = "00000000003", ProductNM = "自由行", Quantity = 100, IsDisable = false }
            };

            DemoViewModel[] productArr = productList.ToArray();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            TestContext.WriteLine(JsonSerializer.Serialize(productArr, options));
            Assert.IsNotNull(productArr);
        }

        [TestCase(Category = "ColloctionsConvert", TestName = "ColloctionsConvert_ListToDictionary")]
        public void ListToDictionary()
        {
            List<DemoViewModel> productList = new List<DemoViewModel>
            {
                new DemoViewModel { ProductID = "00000000001", ProductNM = "機票", Quantity = 100, IsDisable = false },
                new DemoViewModel { ProductID = "00000000002", ProductNM = "訂房", Quantity = 100, IsDisable = false },
                new DemoViewModel { ProductID = "00000000003", ProductNM = "自由行", Quantity = 100, IsDisable = false }
            };

            Dictionary<string, string> productDic = productList.ToDictionary(key => key.ProductID, value => value.ProductNM);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            TestContext.WriteLine(JsonSerializer.Serialize(productDic, options));
            Assert.IsNotNull(productDic);
        }

        [TestCase(Category = "ColloctionsConvert", TestName = "ColloctionsConvert_StringListToSet")]
        public void StringListToSet()
        {
            List<string> strList = new List<string>
            {
                "機票","訂房","自由行","機票","訂房","自由行"
            };

            HashSet<string> strSet = strList.ToHashSet();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            TestContext.WriteLine(JsonSerializer.Serialize(strSet, options));
            Assert.IsNotNull(strSet);
        }

        [TestCase(Category = "ColloctionsConvert", TestName = "ColloctionsConvert_ObjectListToSet")]
        public void ObjectListToSet()
        {
            List<DemoViewModel> productList = new List<DemoViewModel>
            {
                new DemoViewModel { ProductID = "00000000001", ProductNM = "機票", Quantity = 100, IsDisable = false },
                new DemoViewModel { ProductID = "00000000002", ProductNM = "訂房", Quantity = 100, IsDisable = false },
                new DemoViewModel { ProductID = "00000000003", ProductNM = "自由行", Quantity = 100, IsDisable = false },
                new DemoViewModel { ProductID = "00000000001", ProductNM = "機票", Quantity = 100, IsDisable = false },
                new DemoViewModel { ProductID = "00000000002", ProductNM = "訂房", Quantity = 100, IsDisable = false },
                new DemoViewModel { ProductID = "00000000003", ProductNM = "自由行", Quantity = 100, IsDisable = false }
            };

            HashSet<DemoViewModel> productSet = productList.ToHashSet();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            TestContext.WriteLine(JsonSerializer.Serialize(productSet, options));
            Assert.IsNotNull(productSet);
        }

        [TestCase(Category = "ColloctionsConvert", TestName = "ColloctionsConvert_DictionaryToArray")]
        public void DictionaryToArray()
        {
            Dictionary<string, string> productDic = new Dictionary<string, string>
            {
                { "00000000001", "機票" },
                { "00000000002", "訂房" },
                { "00000000003", "自由行" }
            };

            var productArr = productDic.ToArray();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            TestContext.WriteLine(JsonSerializer.Serialize(productArr, options));
            Assert.IsNotNull(productArr);
        }

        [TestCase(Category = "ColloctionsConvert", TestName = "ColloctionsConvert_DictionaryToList")]
        public void DictionaryToList()
        {
            Dictionary<string, string> productDic = new Dictionary<string, string>
            {
                { "00000000001", "機票" },
                { "00000000002", "訂房" },
                { "00000000003", "自由行" }
            };

            List<KeyValuePair<string, string>> productSet = productDic.ToList();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            TestContext.WriteLine(JsonSerializer.Serialize(productSet, options));
            Assert.IsNotNull(productSet);
        }

        [TestCase(Category = "ColloctionsConvert", TestName = "ColloctionsConvert_DictionaryToSet")]
        public void DictionaryToSet()
        {
            Dictionary<string, string> productDic = new Dictionary<string, string>
            {
                { "00000000001", "機票" },
                { "00000000002", "訂房" },
                { "00000000003", "自由行" }
            };

            HashSet<KeyValuePair<string, string>> productSet = productDic.ToHashSet();

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs),
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            TestContext.WriteLine(JsonSerializer.Serialize(productSet, options));
            Assert.IsNotNull(productSet);
        }

        #endregion

        [TestCaseSource(typeof(ModelStateValidateTestCase), nameof(ModelStateValidateTestCase.TestCases))]
        public void ModelStateValidate(DemoViewModel model, bool expectedResult)
        {
            DemoController controller = new DemoController();
            Verification verification = new Verification();
            verification.Validation(controller.ModelState, model);

            bool isValid = controller.ModelState.IsValid;

            Assert.AreEqual(expectedResult, isValid);
        }

        [Test]
        public void ExceptionHandle()
        {
            HttpClient client = null;
            try
            {
                client = new HttpClient();

            }
            catch (Exception ex)
            {
                //log
                EventLog.WriteEntry("Training", ex.Message, EventLogEntryType.Warning);
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }
            Assert.Pass();
        }
    }
}