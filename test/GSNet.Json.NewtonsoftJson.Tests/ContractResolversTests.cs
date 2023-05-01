using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSNet.Json.NewtonsoftJson.ContractResolvers;
using GSNet.Json.NewtonsoftJson.Tests.Model;
using GSNet.Json.NewtonsoftJson.Tests.Model.Enums;
using GSNet.Json.SystemTextJson.Modifiers;
using GSNet.Json.SystemTextJson;
using Newtonsoft.Json;
using Xunit.Abstractions;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using Newtonsoft.Json.Converters;

namespace GSNet.Json.NewtonsoftJson.Tests
{
    public class ContractResolversTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public ContractResolversTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        /// <summary>
        /// 测试忽略属性
        /// </summary>
        [Fact]
        public void Test_Ignore_Property()
        {
            var person = new Person()
            {
                ZhName = "杰尔夫",
                EnName = "Zeref",
                Age = 400,
                Gender = Gender.Male,
                Remark = "黑魔导士的始祖"
            };

            var jsonSerializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings()
            {
                ContractResolver = new ExtendedDefaultContractResolver()
                    .AddIgnorePropertyOrField<Person>(x => x.Remark)
                    .AddIgnorePropertyOrField<Person>(x => x.Gender)
            });

            var jsonStr = jsonSerializer.SerializeObject(person);

            Assert.NotEmpty(jsonStr);
            Assert.DoesNotContain("Remark", jsonStr);
            Assert.DoesNotContain("Gender", jsonStr);

            _outputHelper.WriteLine($"JSON:{jsonStr}");
        }

        /// <summary>
        /// 测试自定义属性的序列化名称
        /// </summary>
        [Fact]
        public void Test_Custom_Property_Or_Field_Name()
        {
            var person = new Person()
            {
                ZhName = "杰尔夫",
                EnName = "Zeref",
                Age = 400,
                Gender = Gender.Male,
                Remark = "黑魔导士的始祖"
            };

            var serializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings()
            {
                ContractResolver = new ExtendedDefaultContractResolver()
                    .ConfigCustomPropertyOrFieldName<Person>(x => x.Remark, "PersonRemark")
            });

            var jsonStr = serializer.SerializeObject(person);

            Assert.NotEmpty(jsonStr);
            Assert.DoesNotContain("\"Remark\"", jsonStr);
            Assert.Contains("\"PersonRemark\"", jsonStr);

            _outputHelper.WriteLine($"JSON:{jsonStr}");

            var personFromJson = serializer.Deserialize<Person>(jsonStr);

            Assert.NotNull(personFromJson);
            Assert.NotNull(personFromJson.Remark);
            Assert.Equal(person.Remark, personFromJson.Remark);
        }

        /// <summary>
        /// 测试支持私有Set
        /// </summary>
        [Fact]
        public void Test_Property_Private_Set()
        {
            var jsonStr = "{\"Grade\":\"大学一年级\",\"Major\":\"软件工程\",\"ZhName\":\"爱德华*王\",\"EnName\":\"Edward Wang\",\"Age\":20,\"Gender\":0,\"Remark\":\"我是一个学生\"}";

            var jsonSerializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new StringEnumConverter()
                }
            });

            var objFromJson = jsonSerializer.Deserialize<Student>(jsonStr);

            Assert.NotNull(objFromJson);
            Assert.Null(objFromJson.Grade);
            Assert.NotNull(objFromJson.Major);

            jsonSerializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new StringEnumConverter()
                },
                ContractResolver = new ExtendedDefaultContractResolver()
                {
                    IsSupportPrivateSetter = true
                }
            });
            objFromJson = jsonSerializer.Deserialize<Student>(jsonStr);

            Assert.NotNull(objFromJson);
            Assert.NotNull(objFromJson.Grade);
        }
    }
}
