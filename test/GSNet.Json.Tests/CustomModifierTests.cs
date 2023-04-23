using GSNet.Json.SystemTextJson.Converters;
using GSNet.Json.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;
using GSNet.Json.Tests.Model;
using Newtonsoft.Json;
using GSNet.Json.SystemTextJson.Modifiers;

namespace GSNet.Json.Tests
{
    /// <summary>
    /// 测试 自定义协议变更
    /// </summary>
    public class CustomModifierTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public CustomModifierTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
        /// <summary>
        /// 测试通过配置 自定义协定 来忽略属性的序列化
        /// </summary>
        [Fact]
        public void Test_Ignore_Property()
        {
            var person = new Person()
            {
                ZhName = "爱德华*王",
                EnName = "Edward Wang",
                Age = 18,
                Remark = "我是一个学生"
            };
            
            var serializer = new SystemTextJsonSerializer(new JsonSerializerOptions()
            {
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    //使用IgnorePropertiesModifier来配置忽略的属性
                    Modifiers = { new IgnorePropertiesModifier().AddIgnoreProperty<Person>(x => x.Remark).ModifyJsonTypeInfo }
                }
            });

            var jsonStr = serializer.SerializeObject(person);

            Assert.NotEmpty(jsonStr);
            Assert.DoesNotContain("Remark", jsonStr);

            _outputHelper.WriteLine($"JSON:{jsonStr}");

            var jsonStr2 = "{\"ZhName\":\"\\u7231\\u5FB7\\u534E*\\u738B\",\"EnName\":\"Edward Wang\"," +
                           "\"Age\":18,\"Remark\":\"\\u6211\\u662F\\u4E00\\u4E2A\\u5B66\\u751F\"}";
            var personFromJson = serializer.Deserialize<Person>(jsonStr2);

            Assert.NotNull(personFromJson);
            Assert.Null(personFromJson.Remark);
        }
    }
}
