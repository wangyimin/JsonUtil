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
            //p.name = null;
            p.age = 21;
            
            S s = new S();
            s.name = "WL";
            //p.s = s;
            p.s = new S[] { s };

            string r = f.stringify(p);
            Trace.WriteLine(r);

            //string str = "{ s:[{ name: \"WL\"}],birthdate: \"2017/08/29 0:00:00\",name: null,age: 21}";
            //Regex regex = new Regex("(?<key>name\\s*:\\s*)(?<value>(\\S*))");
            //MatchCollection mc = regex.Matches(str);

            //string r1 = mc[0].Groups["value"].Value ?? null;

            //Trace.WriteLine(StringUtils.GetJsonValue("name", str));

            r = r.Substring(1, r.Length - 2);
            P reverse = f.Parse<P>(r);
            Trace.WriteLine("End");
        }
    }
}
