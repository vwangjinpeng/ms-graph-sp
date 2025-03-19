using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        // 设置控制台输出编码为UTF-8
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            // 1. 读取apilist.xml文件获取API地址列表List-A
            List<ApiInfo> apiList = ReadApiList("apilist.xml");

            // 2. 读取postdata.xml获取数据列表List-B
            Dictionary<string, string> postDataList = ReadPostData("postdata.xml");

            // 7. 读取存储结果的文件result.xml
            string resultFilePath = "result.xml";
            XDocument resultDoc = new XDocument(new XElement("Results"));

            foreach (var api in apiList)
            {
                try
                {
                    // 3. 从List-A获取单个API地址信息Link-A及http请求方式
                    string linkA = api.Url;
                    string httpMethod = api.Method;

                    if (httpMethod.Equals("GET", StringComparison.OrdinalIgnoreCase))
                    {
                        // 4. 使用Http get访问单个API地址Link-A获取响应数据 Data-A
                        string dataA = await GetDataAsync(linkA);

                        // 5. 获取对应文档中的响应数据 Data-B
                        string dataB = GetExpectedData(linkA);

                        // 6. 对比Data-A和Data-B的结构获得对比结果 Result并显示到控制台
                        string result = CompareData(dataA, dataB);
                        Console.WriteLine(result);

                        // 8. 将Result写入result.xml
                        resultDoc.Root.Add(new XElement("Result",
                            new XElement("API", linkA),
                            new XElement("Method", httpMethod),
                            new XElement("Comparison",
                                new XElement("DataA", dataA),
                                new XElement("DataB", dataB),
                                new XElement("Result", result)
                            )
                        ));
                    }
                    else if (httpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase))
                    {
                        // 9. 从List-B中获取Link-A对应的Postdata-A
                        string postDataA = postDataList[linkA];

                        // 10. 使用http post将数据Postdata-A post到Link-A获取响应数据 Data-C
                        string dataC = await PostDataAsync(linkA, postDataA);

                        // 11. 获取对应文档中的响应数据 Data-D
                        string dataD = GetExpectedData(linkA);

                        // 12. 对比Data-C和Data-D的结构获得对比结果 Result并显示到控制台
                        string result = CompareData(dataC, dataD);
                        Console.WriteLine(result);

                        // 14. 将Result写入result.xml
                        resultDoc.Root.Add(new XElement("Result",
                            new XElement("API", linkA),
                            new XElement("Method", httpMethod),
                            new XElement("Comparison",
                                new XElement("DataC", dataC),
                                new XElement("DataD", dataD),
                                new XElement("Result", result)
                            )
                        ));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"处理API {api.Url} 时发生错误: {ex.Message}");
                    resultDoc.Root.Add(new XElement("Result",
                        new XElement("API", api.Url),
                        new XElement("Method", api.Method),
                        new XElement("Error", ex.Message),
                        new XElement("Response", ex is HttpRequestException httpEx ? httpEx.Message : "无响应数据")
                    ));
                }
            }

            // 16. 保存result.xml文件
            resultDoc.Save(resultFilePath);
            Console.WriteLine("对比结果已写入文件：" + resultFilePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"程序执行过程中发生错误: {ex.Message}");
        }
    }

    static List<ApiInfo> ReadApiList(string filePath)
    {
        try
        {
            // 读取apilist.xml文件并返回API地址列表
            XDocument doc = XDocument.Load(filePath);
            List<ApiInfo> apiList = new List<ApiInfo>();
            foreach (var element in doc.Descendants("API"))
            {
                string url = element.Element("URL").Value;
                string method = element.Element("Method").Value;
                apiList.Add(new ApiInfo { Url = url, Method = method });
            }
            return apiList;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"读取API列表时发生错误: {ex.Message}");
            return new List<ApiInfo>();
        }
    }

    static Dictionary<string, string> ReadPostData(string filePath)
    {
        try
        {
            // 读取postdata.xml文件并返回数据列表
            XDocument doc = XDocument.Load(filePath);
            Dictionary<string, string> postDataList = new Dictionary<string, string>();
            foreach (var element in doc.Descendants("PostData"))
            {
                string api = element.Attribute("API").Value;
                string data = element.Value;
                postDataList.Add(api, data);
            }
            return postDataList;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"读取PostData列表时发生错误: {ex.Message}");
            return new Dictionary<string, string>();
        }
    }

    static async Task<string> GetDataAsync(string url)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GET请求 {url} 时发生错误: {ex.Message}");
            return string.Empty;
        }
    }

    static async Task<string> PostDataAsync(string url, string postData)
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpContent content = new StringContent(postData);
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"POST请求 {url} 时发生错误: {ex.Message}");
            return string.Empty;
        }
    }

    static string GetExpectedData(string api)
    {
        // 模拟获取预期的响应数据
        return "Expected data for " + api;
    }

    static string CompareData(string dataA, string dataB)
    {
        // 模拟对比数据结构
        return dataA == dataB ? "数据匹配" : "数据不匹配";
    }
}

class ApiInfo
{
    public string Url { get; set; }
    public string Method { get; set; }
}
