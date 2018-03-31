﻿using JsonUtil.Codecs;
using JsonUtil.JsonType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JsonUtil.Utils;

namespace JsonUtil.Base
{
    class Factory
    {
        private Dictionary<Type, object> _decodecs = new Dictionary<Type, object>();
        private Dictionary<Type, object> _encodecs = new Dictionary<Type, object>();

        public T GetCodec<T>(Type type)
        {
            Type _type = type ?? throw new ArgumentNullException(nameof(type));

            if (_type.IsArray)
            {
                _type = _type.GetElementType();
            }

            object _codec;
            if (typeof(T) == typeof(Encodec))
            {
                _encodecs.TryGetValue(_type, out _codec);
            }
            else if (typeof(T) == typeof(Decodec))
            {
                _decodecs.TryGetValue(_type, out _codec);
            }
            else
            {
                throw new InvalidOperationException("Unsupport return type[" + typeof(T) + "]");
            }

            return (T)(_codec??throw new InvalidOperationException("Not found type[" + type.Name + "]"));
        }

        public void Builder<T>(object codec)
        {
            object _codec = codec ?? throw new ArgumentNullException(nameof(codec));

            if (_codec is Decodec)
            {
                _decodecs[typeof(T)] = (Decodec)_codec;
            }

            if (_codec is Encodec)
            {
                _encodecs[typeof(T)] = (Encodec)_codec;
            }
        }

        public void Builder()
        {
            Builder<string>(new CodecImpl());
            Builder<int>(new CodecImpl());
            Builder<DateTime>(new CodecImpl());
        }

        public string stringify<T>(T obj)
        {
            // null
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            string r = "{";

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                JsonAttribute attr = prop.GetCustomAttributes<JsonAttribute>(false).FirstOrDefault();
                if (attr != null && attr.decodec != null)
                {
                    // need to use user-defined decoder
                    Decodec impl = (Decodec)Activator.CreateInstance(
                            Type.GetType(attr.decodec.FullName));

                    typeof(Factory).GetMethod("Builder", new Type[] { typeof(Decodec) })
                            .MakeGenericMethod(prop.PropertyType)
                            .Invoke(this, new object[] { impl });
                }

                if (prop.PropertyType.IsArray
                    && (prop.PropertyType.GetElementType().Assembly == Assembly.GetExecutingAssembly()))
                {
                    // array type and not system-defined type
                    Array arr = (Array)prop.GetValue(obj, null);
                    r = r + "\"" + prop.Name + "\":[";
                    foreach (var el in arr)
                    {
                        r = r + stringify(el) + ",";
                    }

                    r = r.Substring(0, r.Length - 1);
                    r = r + "],";
                }
                else if (prop.PropertyType.Assembly == Assembly.GetExecutingAssembly())
                {
                    // not system-defined type
                    r = r + "\"" + prop.Name + "\":" + stringify(prop.GetValue(obj)) + ",";
                }
                else
                {
                    Decodec _decodec = GetCodec<Decodec>(prop.PropertyType);

                    r = r + "\"" + prop.Name + "\":" + _decodec.Convert(prop.GetValue(obj, null)) + ",";
                }
            }

            // remove last comma
            r = r.Substring(0, r.Length - 1);

            r = r + "}";

            return r;
        }

        public T Parse<T>(string s)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));

            if (string.IsNullOrEmpty(s))
            {
                return obj;
            }

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                // get value by specific key
                string part = StringUtils.GetJsonValue(prop.Name, ref s);
                if ("null".Equals(part, StringComparison.CurrentCultureIgnoreCase))
                {
                    prop.SetValue(obj, null);
                }
                else
                {
                    if (prop.PropertyType.IsArray)
                    {
                        if (prop.PropertyType.GetElementType().Assembly != Assembly.GetExecutingAssembly())
                        {
                            string[] lst = part.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            Array arr = Array.CreateInstance(prop.PropertyType.GetElementType(), lst.Length);

                            // system-defined type needs encoder
                            Encodec _encodec = GetCodec<Encodec>(prop.PropertyType);
                            for (int i = 0; i < lst.Length; i++)
                            {
                                var value = typeof(Encodec).GetMethod("Convert", new Type[] { typeof(string) })
                                        .MakeGenericMethod(prop.PropertyType.GetElementType())
                                        .Invoke(_encodec, new object[] { lst[i] });
                                arr.SetValue(value, i);
                            }

                            prop.SetValue(obj, arr, null);
                        }
                        else
                        {
                            // not system-defined type
                            string[] lst = part.Split(new[] { '}', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            Array arr = Array.CreateInstance(prop.PropertyType.GetElementType(), lst.Length);

                            for (int i = 0; i < lst.Length; i++)
                            {
                                var value = typeof(Factory).GetMethod("Parse", new Type[] { typeof(string) })
                                        .MakeGenericMethod(prop.PropertyType.GetElementType())
                                        .Invoke(this, new object[] { lst[i] });
                                arr.SetValue(value, i);
                            }
                            prop.SetValue(obj, arr, null);
                        }
                    }
                    else if (prop.PropertyType.Assembly == Assembly.GetExecutingAssembly())
                    {
                        // not system defined type
                        prop.SetValue(obj,
                                        typeof(Factory).GetMethod("Parse", new Type[] { typeof(string) })
                                            .MakeGenericMethod(prop.PropertyType)
                                            .Invoke(this, new object[] { part }),
                                        null);

                    }
                    else
                    {
                        Encodec _encodec = GetCodec<Encodec>(prop.PropertyType);

                        var value = typeof(Encodec).GetMethod("Convert", new Type[] { typeof(string) })
                                    .MakeGenericMethod(prop.PropertyType)
                                    .Invoke(_encodec, new object[] { part });
                        prop.SetValue(obj, value, null);
                    }
                }
            }
            return obj;
        }
    }
}
