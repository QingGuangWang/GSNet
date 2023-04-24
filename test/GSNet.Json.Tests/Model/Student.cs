using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.Json.Tests.Model
{
    public class Student : Person
    {
        /// <summary>
        /// </summary>
        private Student()
        {
        }

        /// <summary>
        /// 基于年纪和专业构造
        /// </summary>
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
