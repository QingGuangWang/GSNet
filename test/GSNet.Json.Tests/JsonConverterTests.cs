using GSNet.Json.SystemTextJson;
using GSNet.Json.SystemTextJson.Converters;
using GSNet.Json.Tests.Model;
using GSNet.Json.Tests.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace GSNet.Json.Tests
{
    /// <summary>
    /// 测试 JSON数据转化器
    /// </summary>
    public class JsonConverterTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public JsonConverterTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        /// <summary>
        /// 测试 自带的 字符串枚举转换器<see cref="JsonStringEnumConverter"/>
        /// </summary>
        [Fact]
        public void Test_JsonStringEnumConverter()
        {
            var person = new Person()
            {
                ZhName = "丽萨",
                EnName = "Lisa",
                Age = 18,
                Gender = Gender.Female,
                Remark = "Student"
            };

            var serializerWithoutConverter = new SystemTextJsonSerializer(new JsonSerializerOptions());

            var serializerWithJsonStringEnumConverter = new SystemTextJsonSerializer(new JsonSerializerOptions()
            {
                Converters = { new JsonStringEnumConverter() }
            });

            var json1 = serializerWithoutConverter.Serialize(person);
            var json2 = serializerWithJsonStringEnumConverter.Serialize(person);

            Assert.NotEqual(json1, json2);
            Assert.Contains("\"Gender\":1", json1);
            Assert.Contains("\"Gender\":\"Female\"", json2);
            
            _outputHelper.WriteLine($"JSON Serializer Without StringBooleanConverter And WriteToString:  {json1}");
            _outputHelper.WriteLine($"JSON Serializer With StringBooleanConverter And WriteToString:  {json2}");

            //没有配置 JsonStringEnumConverter 的序列化器 序列化 字符串枚举的JSON, 会提示错误（The JSON value could not be converted to ）
            Assert.Throws<System.Text.Json.JsonException>(() => serializerWithoutConverter.Deserialize<Person>(json2));

            var person2FromJson2 = serializerWithJsonStringEnumConverter.Deserialize<Person>(json2);

            Assert.Equal(person2FromJson2.Gender, person.Gender);

            //配置 JsonStringEnumConverter 的序列化器 序列化 数字枚举值的JSON, 也是兼容的
            var person3FromJson2 = serializerWithJsonStringEnumConverter.Deserialize<Person>(json1);

            Assert.Equal(person3FromJson2.Gender, person.Gender);
        }

        /// <summary>
        /// 测试 自定义的 字符串布尔值转换器<see cref="StringBooleanConverter"/>
        /// </summary>
        [Fact]
        public void Test_StringBooleanConverter()
        {
            var serializerWithoutConverter = new SystemTextJsonSerializer(new JsonSerializerOptions());

            var serializerWithStringBooleanConverter = new SystemTextJsonSerializer(new JsonSerializerOptions()
            {
                Converters = { new StringBooleanConverter(), new JsonStringEnumConverter() }
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
