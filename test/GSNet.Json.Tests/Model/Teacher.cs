﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NewtonsoftJson
namespace GSNet.Json.NewtonsoftJson.Tests.Model
#else
namespace GSNet.Json.Tests.Model
#endif
{
    /// <summary>
    /// 教师
    /// </summary>
    public class Teacher : Person
    {
        public Teacher()
        {

        }

        /// <summary>
        /// 所属学校(教师可以是多个学校)
        /// </summary>
        public IList<string> Schools { get; set; }

        /// <summary>
        /// 职称
        /// </summary>
        public string ProfessionalTitle { get; set; }
    }
}
