using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using JsonUtil.Codecs;
using JsonUtil.Base;
using System.Reflection;
using JsonUtil.Demo;
using JsonUtil.JsonType;
using System.Text.RegularExpressions;
using JsonUtil.Utils;

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
            
            S s1 = new S();
            s1.name = "WL1";
            s1.date = DateTime.Today;

            S s2 = new S();
            s2.name = "WL2";
            s2.date = DateTime.Today;
            p.s = new S[] { s1, s2 };

            string r = f.stringify(p);
            Trace.WriteLine(r);
            
            r = r.Substring(1, r.Length - 2);
            P reverse = f.Parse<P>(r);
            Trace.WriteLine("End");
        }
    }
}
