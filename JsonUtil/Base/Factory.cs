using JsonUtil.Decodecs;
using JsonUtil.JsonType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JsonUtil.Base
{
    class Factory
    {
        private Dictionary<Type, object> _decodecs = new Dictionary<Type, object>();

        public Decodec GetDecodec(Type type)
        {
            Type _type = type ?? throw new ArgumentNullException(nameof(type));

            if (_type.IsArray)
            {
                _type = _type.GetElementType();
            }

            _decodecs.TryGetValue(_type, out var _decodec);

            return (Decodec)_decodec ?? throw new InvalidOperationException("Unsupport type[" + type.Name + "]");
        }

        public void Builder<T>(Decodec _decode)
        {
            _decodecs[typeof(T)] = _decode;
        }

        public void Builder()
        {
            Builder<string>(new DecodecImpl());
            Builder<int>(new DecodecImpl());
            Builder<DateTime>(new DecodecImpl());
        }

        public string Parse<T>(T obj)
        {
            if (obj == null)
            {
                return "{}";
            }

            string r = "{";

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                JsonAttribute attr = prop.GetCustomAttributes<JsonAttribute>(false).FirstOrDefault();
                if (attr != null && attr.decodec != null)
                {
                    Decodec impl = (Decodec)Activator.CreateInstance(
                            Type.GetType(attr.decodec.FullName));

                    typeof(Factory).GetMethod("Builder", new Type[] { typeof(Decodec) })
                            .MakeGenericMethod(prop.PropertyType)
                            .Invoke(this, new object[] { impl });
                }

                if (prop.GetType().IsClass && prop.PropertyType.Assembly == Assembly.GetExecutingAssembly())
                {
                    r = r + prop.Name + ":" + Parse(prop.GetValue(obj)) + ",";
                }
                else
                {
                    Decodec _decodec = GetDecodec(prop.PropertyType);

                    r = r + prop.Name + ":" + _decodec.Convert(prop.GetValue(obj, null)) + ",";
                }
            }

            if (r[r.Length - 1] == ',')
                r = r.Substring(0, r.Length - 1);
            r = r + "}";

            return r;
        }

    }
}
