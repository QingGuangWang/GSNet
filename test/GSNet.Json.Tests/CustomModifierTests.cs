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
                    Modifiers =
                    {
                        new IgnorePropertiesModifier()
                            //忽略备注
                            .AddIgnoreProperty<Person>(x => x.Remark)
                            .ModifyJsonTypeInfo
                    }
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

        /// <summary>
        /// 测试通过配置 ParameterlessConstructorModifier 来实现私有构造方法的调用
        /// </summary>
        [Fact]
        public void Test_Parameterless_Constructor()
        {
            var student = new Student("大学一年级", "软件工程")
            {
                ZhName = "爱德华*王",
                EnName = "Edward Wang",
                Age = 20,
                Remark = "我是一个学生"
            };

            var serializer = new SystemTextJsonSerializer(new JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            });

            var jsonStr = serializer.SerializeObject(student);

            Assert.NotEmpty(jsonStr);

            _outputHelper.WriteLine($"JSON:{jsonStr}");

            Assert.Throws<System.InvalidOperationException>(() =>
            {
                //Student没有公开的无参数构造方法，也没有 构造参数与属性名称完全匹配的 构造方法
                serializer.Deserialize<Student>(jsonStr);

                //会报错误
                //Each parameter in the deserialization constructor on type 'GSNet.Json.Tests.Model.Student' must bind to an object property or field on deserialization.
                //Each parameter name must match with a property or field on the object.
                //Fields are only considered when 'JsonSerializerOptions.IncludeFields' is enabled. The match can be case-insensitive.
            });

            serializer = new SystemTextJsonSerializer(new JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    //使用ParameterlessConstructorModifier来配置使用无参数构造方法去构造实例
                    Modifiers =
                    {
                        new ParameterlessConstructorModifier().ModifyJsonTypeInfo
                    }
                }
            });

            var objFromJson = serializer.Deserialize<Student>(jsonStr);

            Assert.NotNull(objFromJson);
            Assert.Null(objFromJson.Grade); //Grade 是私有set
            Assert.NotNull(objFromJson.Major);

        }

        [Fact]
        public void Test_Uninitialized_Object()
        {
            var jsonStr = "{\"Grade\":\"大学一年级\",\"Major\":\"软件工程\",\"ZhName\":\"爱德华*王\",\"EnName\":\"Edward Wang\",\"Age\":20,\"Remark\":\"我是一个学生\"}";

            var serializer = new SystemTextJsonSerializer(new JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers =
                    {
                        new SpecificObjectConstructorModifier().AddCreateUninitializedObject<Student>().ModifyJsonTypeInfo,
                    }
                }
            });

            var objFromJson = serializer.Deserialize<Student>(jsonStr);

            Assert.NotNull(objFromJson);
            Assert.Null(objFromJson.Grade); //Grade 是私有set
            Assert.NotNull(objFromJson.Major);
        }

        [Fact]
        public void Test_Property_Private_Set()
        {
            var jsonStr = "{\"Grade\":\"大学一年级\",\"Major\":\"软件工程\",\"ZhName\":\"爱德华*王\",\"EnName\":\"Edward Wang\",\"Age\":20,\"Remark\":\"我是一个学生\"}";

            var serializer = new SystemTextJsonSerializer(new JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers =
                    {
                        new ParameterlessConstructorModifier().ModifyJsonTypeInfo,
                        new PropertiesWithPrivateSetModifier().ModifyJsonTypeInfo,
                    }
                }
            }); 

            var objFromJson = serializer.Deserialize<Student>(jsonStr);

            Assert.NotNull(objFromJson);
            Assert.NotNull(objFromJson.Grade); //配置了 PropertiesWithPrivateSetModifier，则非空
            Assert.NotNull(objFromJson.Major);
        }

    }
}
