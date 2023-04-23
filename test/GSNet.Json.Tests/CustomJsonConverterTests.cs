using GSNet.Json.SystemTextJson;
using GSNet.Json.SystemTextJson.Converters;
using GSNet.Json.Tests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace GSNet.Json.Tests
{
    /// <summary>
    /// 测试 自定义转化器
    /// </summary>
    public class CustomJsonConverterTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public CustomJsonConverterTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        /// <summary>
        /// 测试字符串布尔值转换器<see cref="StringBooleanConverter"/>
        /// </summary>
        [Fact]
        public void Test_StringBooleanConverter()
        {
            var serializerWithoutConverter = new SystemTextJsonSerializer(new JsonSerializerOptions());

            var serializerWithStringBooleanConverter = new SystemTextJsonSerializer(new JsonSerializerOptions()
            {
                Converters = { new StringBooleanConverter() }
            });

            var serializerWithStringBooleanConverterAndWriteToString = new SystemTextJsonSerializer(new JsonSerializerOptions()
            {
                Converters = { new StringBooleanConverter(true) }
            });

            var project = new Project()
            {
                Name = "大项目", Funds = 234212.11M, IsCompleted = true
            };

            var json1 = serializerWithoutConverter.Serialize(project);
            var json2 = serializerWithStringBooleanConverter.Serialize(project);
            var json3 = serializerWithStringBooleanConverterAndWriteToString.Serialize(project);

            Assert.Equal(json1, json2);
            Assert.NotEqual(json1, json3);

            _outputHelper.WriteLine($"JSON Serializer With StringBooleanConverter And WriteToString:  {json3}");

            //没有StringBooleanConverter 会抛出
            Assert.Throws<JsonException>(() =>
            {
                serializerWithoutConverter.Deserialize<Project>(json3);
            });

            var project1FromJson3 = serializerWithStringBooleanConverter.Deserialize<Project>(json3);
            var project2FromJson3 = serializerWithStringBooleanConverterAndWriteToString.Deserialize<Project>(json3);

            Assert.Equal(project1FromJson3.Name, project.Name);
            Assert.Equal(project2FromJson3.Name, project.Name);
            Assert.Equal(project1FromJson3.Funds, project.Funds);
            Assert.Equal(project2FromJson3.Funds, project.Funds);
            Assert.Equal(project1FromJson3.IsCompleted, project.IsCompleted);
            Assert.Equal(project2FromJson3.IsCompleted, project.IsCompleted);

        }
    }
}
