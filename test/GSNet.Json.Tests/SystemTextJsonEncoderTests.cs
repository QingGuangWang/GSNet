using GSNet.Json.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace GSNet.Json.Tests
{
    /// <summary>
    /// 测试SystemTextJson 在不同编码下的序列化
    /// </summary>
    public class SystemTextJsonEncoderTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public SystemTextJsonEncoderTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        /// <summary>
        /// 序列化特殊字符测试 --》 <![CDATA[ "你好：!@#$%^&*()_+！@#￥%……&*（）、~" ]]>
        /// <para>
        /// 不指定 <see cref="JsonSerializerOptions.Encoder"/>
        /// </para>
        /// 中文和一些符号会被转码显示
        /// </summary>
        [Fact]
        public void Test_Serialize_SpecialSymbol_Without_Encoder()
        {
            var obj = new
            {
                Symbol = "你好：!@#$%^&*()_+！@#￥%……&*（）、~"
            };

            //不配置 Encoder
            var converter = new SystemTextJsonConverter(new JsonSerializerOptions());

            // {"Symbol":"\u4F60\u597D\uFF1A!@#$%^\u0026*()_\u002B\uFF01@#\uFFE5%\u2026\u2026\u0026*\uFF08\uFF09\u3001~"}
            var jsonString = converter.SerializeObject(obj);

            _outputHelper.WriteLine($"不配置 Encoder:  {jsonString}");

            Assert.DoesNotContain(obj.Symbol, jsonString);
        }
        
        /// <summary>
        /// 序列化特殊字符测试 --》 <![CDATA[ "你好：!@#$%^&*()_+！@#￥%……&*（）、~" ]]>
        /// <para>
        /// 指定 <see cref="JsonSerializerOptions.Encoder"/> 是 UnicodeRanges.All
        /// </para>
        /// 中文和一些符号会正常显示，但是一些字符还是会转码，如 <![CDATA[ & ]]>
        /// </summary>
        [Fact]
        public void Test_Serialize_SpecialSymbol_With_Unicode_Encoder()
        {
            var obj = new
            {
                Symbol = "你好：!@#$%^&*()_+！@#￥%……&*（）、~"
            };

            // 配置 Encoder
            var converter = new SystemTextJsonConverter(new JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All),
            });

            // {"Symbol":"你好：!@#$%^\u0026*()_\u002B！@#￥%……\u0026*（）、~"}
            var jsonString = converter.SerializeObject(obj);

            _outputHelper.WriteLine($"配置UnicodeRanges.All Encoder:  {jsonString}");

            Assert.DoesNotContain(obj.Symbol, jsonString);
        }

        /// <summary>
        /// 序列化特殊字符测试 --》 <![CDATA[ "你好：!@#$%^&*()_+！@#￥%……&*（）、~" ]]>
        /// <para>
        /// 指定 <see cref="JsonSerializerOptions.Encoder"/> 是 UnsafeRelaxedJsonEscaping
        /// </para>
        /// 中文和符号会正常显示
        /// </summary>
        [Fact]
        public void Test_Serialize_SpecialSymbol_With_UnsafeRelaxedJsonEscaping_Encoder()
        {
            var obj = new
            {
                Symbol = "你好：!@#$%^&*()_+！@#￥%……&*（）、~"
            };

            // 配置 Encoder
            var converter = new SystemTextJsonConverter(new JsonSerializerOptions()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            // {"Symbol":"你好：!@#$%^&*()_+！@#￥%……&*（）、~"}
            var jsonString = converter.SerializeObject(obj);

            _outputHelper.WriteLine($"配置UnsafeRelaxedJsonEscaping Encoder:  {jsonString}");

            Assert.Contains(obj.Symbol, jsonString);
        }
    }
}
