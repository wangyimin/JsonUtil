using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using JsonUtil.Decodecs;
using JsonUtil.Base;
using System.Reflection;
using JsonUtil.Demo;
using JsonUtil.JsonType;

namespace JsonUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            Factory f = new Factory();
            f.Builder();
            
            P p = new P();
            p.birthdate = DateTime.Today;
            p.name = new string[] { "Wang", "Li" };
            p.age = 21;
            
            S s = new S();
            s.name = "WL";
            p.s = s;

            string r = f.Parse(p);

            Trace.WriteLine(r);
        }
    }
}
