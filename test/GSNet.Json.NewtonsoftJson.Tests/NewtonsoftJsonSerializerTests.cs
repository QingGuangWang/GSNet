using GSNet.Json.SystemTextJson;
using System.Text.Json;
using GSNet.Json.NewtonsoftJson.Tests.Model;
using GSNet.Json.NewtonsoftJson.Tests.Model.Enums;
using Xunit.Abstractions;
using System.Text;
using GSNet.Common.Extensions;

namespace GSNet.Json.NewtonsoftJson.Tests
{
    /// <summary>
    /// ���� NewtonsoftJsonSerializer
    /// </summary>
    public class NewtonsoftJsonSerializerTests
    {
        private readonly ITestOutputHelper _outputHelper;

        private readonly IJsonSerializer _jsonSerializer;

        public NewtonsoftJsonSerializerTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _jsonSerializer = new NewtonsoftJsonSerializer();
        }

        /// <summary>
        /// ��Ҫ���л��Ķ���
        /// </summary>
        private readonly Person _person = new Person()
        {
            ZhName = "�ܶ���",
            EnName = "Zeref",
            Age = 400,
            Gender = Gender.Male,
            Remark = "��ħ��ʿ��ʼ��"
        };

        /// <summary>
        /// Ԥ�ڵ�JSON�ַ���
        /// </summary>
        private const string ExpectedJsonStr = "{\"ZhName\":\"�ܶ���\",\"EnName\":\"Zeref\",\"Age\":400,\"Gender\":0,\"Remark\":\"��ħ��ʿ��ʼ��\"}";

        /// <summary>
        /// ���� <see cref="IJsonSerializer.Serialize{T}"/> ����
        /// </summary>
        [Fact]
        public void Test_Serialize()
        {
            var jsonStr = _jsonSerializer.Serialize(_person);

            _outputHelper.WriteLine(jsonStr);

            Assert.NotEmpty(jsonStr);
            Assert.Equal(ExpectedJsonStr, jsonStr);
        }

        /// <summary>
        /// ���� <see cref="IJsonSerializer.SerializeObject"/> ����
        /// </summary>
        [Fact]
        public void Test_SerializeObject()
        {
            var jsonStr = _jsonSerializer.SerializeObject(_person);

            Assert.NotEmpty(jsonStr);
            Assert.Equal(ExpectedJsonStr, jsonStr);
        }

        /// <summary>
        /// ���� <see cref="IJsonSerializer.SerializeToStream(Object,Stream)"/> ����
        /// </summary>
        [Fact]
        public void Test_SerializeToStream()
        {
            var memoryStream = new MemoryStream();

            _jsonSerializer.SerializeToStream(_person, memoryStream);

            var bytes = memoryStream.ToArray();
            var jsonStr = Encoding.UTF8.GetString(bytes);

            Assert.NotEmpty(jsonStr);
            Assert.Equal(ExpectedJsonStr, jsonStr);
        }

        /// <summary>
        /// ���� <see cref="IJsonSerializer.SerializeToStream(Type,Object,Stream)"/> ����
        /// </summary>
        [Fact]
        public void Test_SerializeToStream_Specific_Type()
        {
            var memoryStream = new MemoryStream();

            _jsonSerializer.SerializeToStream(typeof(Person), _person, memoryStream);

            var bytes = memoryStream.ToArray();
            var jsonStr = Encoding.UTF8.GetString(bytes);

            Assert.NotEmpty(jsonStr);
            Assert.Equal(ExpectedJsonStr, jsonStr);
        }

        /// <summary>
        /// ���� <see cref="IJsonSerializer.Deserialize{T}"/> ����
        /// </summary>
        [Fact]
        public void Test_Deserialize()
        {
            var person = _jsonSerializer.Deserialize<Person>(ExpectedJsonStr);

            Assert.NotNull(person);
            Assert.Equal(_person.EnName, person.EnName);
            Assert.Equal(_person.ZhName, person.ZhName);
            Assert.Equal(_person.Age, person.Age);
            Assert.Equal(_person.Remark, person.Remark);
        }

        /// <summary>
        /// ���� <see cref="IJsonSerializer.Deserialize(System.Type,string)"/> ����
        /// </summary>
        [Fact]
        public void Test_Deserialize_Specific_Type()
        {
            var obj = _jsonSerializer.Deserialize(typeof(Person), ExpectedJsonStr);

            Assert.NotNull(obj);
            Assert.True(obj is Person);

            var person = obj as Person;

            Assert.Equal(_person.EnName, person.EnName);
            Assert.Equal(_person.ZhName, person.ZhName);
            Assert.Equal(_person.Age, person.Age);
            Assert.Equal(_person.Remark, person.Remark);
        }

        /// <summary>
        /// ���� <see cref="IJsonSerializer.Deserialize(System.Type,Stream)"/> ����
        /// </summary>
        [Fact]
        public void Test_Deserialize_Specific_Type_From_Stream()
        {
            var memoryStream = new MemoryStream(ExpectedJsonStr.GetBytes());

            var obj = _jsonSerializer.Deserialize(typeof(Person), memoryStream);

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