using GSNet.Json.SystemTextJson;
using System.Text.Json;
using GSNet.Json.Tests.Model;
using Xunit.Abstractions;
using System.Text;
using GSNet.Common.Extensions;

namespace GSNet.Json.Tests
{
    /// <summary>
    /// 测试SystemTextJsonConverter
    /// </summary>
    public class SystemTextJsonConverterTests
    {
        private readonly ITestOutputHelper _outputHelper;

        private readonly IJsonConverter _jsonConverter;

        /// <summary>
        /// 需要序列化的对象
        /// </summary>
        private readonly Person _person= new Person()
        {
            ZhName = "爱德华*王",
            EnName = "Edward Wang",
            Age = 18,
            Remark = "我是一个学生"
        };

        /// <summary>
        /// 预期的JSON字符串
        /// </summary>
        private const string ExpectedJsonStr = "{\"ZhName\":\"\\u7231\\u5FB7\\u534E*\\u738B\",\"EnName\":\"Edward Wang\"," +
                                               "\"Age\":18,\"Remark\":\"\\u6211\\u662F\\u4E00\\u4E2A\\u5B66\\u751F\"}";

        public SystemTextJsonConverterTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _jsonConverter = new SystemTextJsonConverter(new JsonSerializerOptions());
        }

        /// <summary>
        /// 测试 <see cref="IJsonConverter.Serialize{T}"/> 方法
        /// </summary>
        [Fact]
        public void Test_Serialize()
        {
            var jsonStr = _jsonConverter.Serialize(_person);

            Assert.NotEmpty(jsonStr);
            Assert.Equal(ExpectedJsonStr, jsonStr);
        }

        /// <summary>
        /// 测试 <see cref="IJsonConverter.SerializeObject"/> 方法
        /// </summary>
        [Fact]
        public void Test_SerializeObject()
        {
            var jsonStr = _jsonConverter.SerializeObject(_person);
            
            Assert.NotEmpty(jsonStr);
            Assert.Equal(ExpectedJsonStr, jsonStr);
        }

        /// <summary>
        /// 测试 <see cref="IJsonConverter.SerializeToStream(Object,Stream)"/> 方法
        /// </summary>
        [Fact]
        public void Test_SerializeToStream()
        {
            var memoryStream = new MemoryStream(); 
            
            _jsonConverter.SerializeToStream(_person, memoryStream);

            var bytes = memoryStream.ToArray();
            var jsonStr = Encoding.UTF8.GetString(bytes);

            Assert.NotEmpty(jsonStr);
            Assert.Equal(ExpectedJsonStr, jsonStr);
        }

        /// <summary>
        /// 测试 <see cref="IJsonConverter.SerializeToStream(Type,Object,Stream)"/> 方法
        /// </summary>
        [Fact]
        public void Test_SerializeToStream_Specific_Type()
        {
            var memoryStream = new MemoryStream();

            _jsonConverter.SerializeToStream(typeof(Person), _person, memoryStream);

            var bytes = memoryStream.ToArray();
            var jsonStr = Encoding.UTF8.GetString(bytes);

            Assert.NotEmpty(jsonStr);
            Assert.Equal(ExpectedJsonStr, jsonStr);
        }
        
        /// <summary>
        /// 测试 <see cref="IJsonConverter.Deserialize{T}"/> 方法
        /// </summary>
        [Fact]
        public void Test_Deserialize()
        {
            var person = _jsonConverter.Deserialize<Person>(ExpectedJsonStr);

            Assert.NotNull(person);
            Assert.Equal(_person.EnName, person.EnName);
            Assert.Equal(_person.ZhName, person.ZhName);
            Assert.Equal(_person.Age, person.Age);
            Assert.Equal(_person.Remark, person.Remark);
        }

        /// <summary>
        /// 测试 <see cref="IJsonConverter.Deserialize(System.Type,string)"/> 方法
        /// </summary>
        [Fact] public void Test_Deserialize_Specific_Type()
        {
            var obj = _jsonConverter.Deserialize(typeof(Person), ExpectedJsonStr);

            Assert.NotNull(obj);
            Assert.True(obj is Person);

            var person = obj as Person;

            Assert.Equal(_person.EnName, person.EnName);
            Assert.Equal(_person.ZhName, person.ZhName);
            Assert.Equal(_person.Age, person.Age);
            Assert.Equal(_person.Remark, person.Remark);
        }

        /// <summary>
        /// 测试 <see cref="IJsonConverter.Deserialize(System.Type,Stream)"/> 方法
        /// </summary>
        [Fact]
        public void Test_Deserialize_Specific_Type_From_Stream()
        {
            var memoryStream = new MemoryStream(ExpectedJsonStr.GetBytes());

            var obj = _jsonConverter.Deserialize(typeof(Person), memoryStream);

            Assert.NotNull(obj);
            Assert.True(obj is Person);

            var person = obj as Person;

            Assert.Equal(_person.EnName, person.EnName);
            Assert.Equal(_person.ZhName, person.ZhName);
            Assert.Equal(_person.Age, person.Age);
            Assert.Equal(_person.Remark, person.Remark);
        }
    }
}