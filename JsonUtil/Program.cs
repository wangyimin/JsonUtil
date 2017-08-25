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
            
            /*
            //f.Builder<string>(new DecodecImpl());
            typeof(Factory)
                .GetMethod("Builder").MakeGenericMethod(typeof(string)).Invoke(f, new object[] { new DecodecImpl() });

            f.Builder<int>(new DecodecImpl());
            
            Decodec m = f.GetDecodec(typeof(int));
            Trace.WriteLine(m.Parse(21));

            Decodec _decodec = (Decodec)typeof(Factory)
                .GetMethod("GetDecodec")
                .Invoke(f, new Type[] {typeof(bool)});
            Trace.WriteLine(_decodec.Parse("ABC"));
            */
            P p = new P();
            p.birthdate = DateTime.Today;
            p.name = new string[] { "Wang", "Li" };
            p.age = 21;
            
            S s = new S();
            s.name = "WL";
            p.s = s;
            
            List<Node> nodes = new List<Node>();
            string r = f.Parse(p);

            Trace.WriteLine(r);
        }
    }
}
