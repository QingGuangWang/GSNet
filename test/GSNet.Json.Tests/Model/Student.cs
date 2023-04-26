using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Json.Tests.Model
{
    /// <summary>
    /// 学生
    /// </summary>
    public class Student : Person
    {
        /// <summary>
        /// 私有构造方法
        /// </summary>
        private Student()
        {
        }

        /// <summary>
        /// 基于年级和专业构造
        /// </summary>
        /// <param name="studentGrade">学生的年级</param>
        /// <param name="major">专业</param>
        public Student(string studentGrade, string major)
        {
            Grade = studentGrade;
            Major = major;
        }

        /// <summary>
        /// 年级
        /// </summary>
        public string Grade { get; private set; }

        /// <summary>
        /// 专业
        /// </summary>
        public string Major { get; set; }
    }
}
