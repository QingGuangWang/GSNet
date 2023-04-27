using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Common.Helper
{
    /// <summary>
    /// 针对表达式树常用的一些辅助方法
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// 从表示访问成员的Lambda表达式(如x => x.Name， x => x.Date), 获取其表达的指定类型（参数<typeparamref name="TSource"/>）下的成员（属性或者字段）的信息
        /// </summary>
        /// <typeparam name="TSource">类型</typeparam>
        /// <param name="expression">表示访问成员的Lambda表达式， 如 x => x.Name </param>
        /// <returns>MemberInfo对象</returns>
        /// <exception cref="ArgumentException">如果表达式不是访问成员（属性或者字段），则抛出此错误</exception>
        public static MemberInfo GetMemberInfo<TSource>(Expression<Func<TSource, object>> expression)
        {
            return GetMemberInfo<TSource, object>(expression);
        }

        /// <summary>
        /// 从表示访问成员的Lambda表达式(如x => x.Name， x => x.Date), 获取其表达的指定类型（参数<typeparamref name="TSource"/>）下的成员（属性或者字段）的信息
        /// </summary>
        /// <typeparam name="TSource">类型</typeparam>
        /// <typeparam name="TMember">成员（属性或者字段）的类型</typeparam>
        /// <param name="expression">表示访问成员的Lambda表达式， 如 x => x.Name </param>
        /// <returns>MemberInfo对象</returns>
        /// <exception cref="ArgumentException">如果表达式不是访问成员（属性或者字段），则抛出此错误</exception>
        public static MemberInfo GetMemberInfo<TSource, TMember>(Expression<Func<TSource, TMember>> expression)
        {
            //获取LambdaExpression 的主体 如x => x.Name  则获取到 x.Name
            // x.Name 正常情况下是 MemberExpression 或者 UnaryExpression 
            var lambdaExpressionBody = expression.Body;

            //在表达式输入的正确的 下基本是 MemberExpression  
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member;
            }
            //部分情况下，Body会是 UnaryExpression，其 属性Operand 是 MemberExpression
            else if (expression.Body is UnaryExpression { Operand: MemberExpression operandMemberExpression })
            {
                return operandMemberExpression.Member;
            }
            
            throw new ArgumentException(@"The lambda expression is not a member access", nameof(expression));
        }

        /// <summary>
        /// 从表示访问属性的Lambda表达式(如x => x.Name， x => x.Date), 获取其表达的指定类型（参数<typeparamref name="TSource"/>）下的成员属性的信息
        /// </summary>
        /// <typeparam name="TSource">类型</typeparam>
        /// <param name="expression">表示访问属性的Lambda表达式， 如 x => x.Name </param>
        /// <returns>PropertyInfo对象</returns>
        /// <exception cref="ArgumentException">如果表达式不是访问成员，或者访问的成员不是属性，则抛出此错误</exception>
        public static PropertyInfo GetPropertyInfo<TSource>(Expression<Func<TSource, object>> expression)
        {
            return GetPropertyInfo<TSource, object>(expression);
        }

        /// <summary>
        /// 从表示访问属性的Lambda表达式(如x => x.Name， x => x.Date), 获取其表达的指定类型（参数<typeparamref name="TSource"/>）下的成员属性的信息
        /// </summary>
        /// <typeparam name="TSource">类型</typeparam>
        /// <typeparam name="TMember">成员属性的类型</typeparam>
        /// <param name="expression">表示访问属性的Lambda表达式， 如 x => x.Name </param>
        /// <returns>PropertyInfo对象</returns>
        /// <exception cref="ArgumentException">如果表达式不是访问成员，或者访问的成员不是属性，则抛出此错误</exception>
        public static PropertyInfo GetPropertyInfo<TSource, TMember>(Expression<Func<TSource, TMember>> expression)
        {
            var memberInfo = GetMemberInfo(expression);

            if (memberInfo is PropertyInfo propertyInfo)
            {
                return propertyInfo;
            }

            //抛出错误
            throw new ArgumentException(@"The member is not a property", nameof(expression));
        }

        /// <summary>
        /// 从表示访问字段的Lambda表达式(如x => x.Name， x => x._date), 获取其表达的指定类型（参数<typeparamref name="TSource"/>）下的成员字段的信息
        /// </summary>
        /// <typeparam name="TSource">类型</typeparam>
        /// <param name="expression">表示访问字段的Lambda表达式， 如 x => x.Name </param>
        /// <returns>FieldInfo对象</returns>
        /// <exception cref="ArgumentException">如果表达式不是访问成员，或者访问的成员不是字段，则抛出此错误</exception>
        public static FieldInfo GetFieldInfo<TSource>(Expression<Func<TSource, object>> expression)
        {
            return GetFieldInfo<TSource, object>(expression);
        }

        /// <summary>
        /// 从表示访问字段的Lambda表达式(如x => x.Name， x => x._date), 获取其表达的指定类型（参数<typeparamref name="TSource"/>）下的成员字段的信息
        /// </summary>
        /// <typeparam name="TSource">类型</typeparam>
        /// <typeparam name="TMember">成员字段的类型</typeparam>
        /// <param name="expression">表示访问字段的Lambda表达式， 如 x => x.Name </param>
        /// <returns>FieldInfo对象</returns>
        /// <exception cref="ArgumentException">如果表达式不是访问成员，或者访问的成员不是字段，则抛出此错误</exception>
        public static FieldInfo GetFieldInfo<TSource, TMember>(Expression<Func<TSource, TMember>> expression)
        {
            var memberInfo = GetMemberInfo(expression);

            if (memberInfo is FieldInfo fieldInfo)
            {
                return fieldInfo;
            }

            //抛出错误
            throw new ArgumentException(@"The member is not a field", nameof(expression));
        }
    }
}
