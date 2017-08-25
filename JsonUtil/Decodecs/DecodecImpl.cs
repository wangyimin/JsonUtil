using JsonUtil.Base;
using System;

namespace JsonUtil.Decodecs
{
    class DecodecImpl : Decodec
    {
        public string Convert<T>(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            if (obj is string)
            {
                return "\"" + obj + "\"";
            }
            else if (obj is Int32)
            {
                return obj.ToString();
            }
            else if (obj is DateTime)
            {
                return "\"" + ((DateTime)(object)obj).ToString("yyyy/MM/dd") + "\""; 
            }
            else if (obj is Array)
            {
                var arr = obj as Array;
                string r = "[";
                foreach (var el in arr)
                {
                    r = r + Convert(el) + ", ";
                }
                if (r.Length > 1)
                {
                    r = r.Substring(0, r.Length - 2);
                }

                r = r + "]";

                return r;
            }
            else
            {
                throw new InvalidOperationException("Unsupport object type["
                    + obj.GetType().Namespace + "." + obj.GetType().Name + "]");
            }
        }
    }
}
