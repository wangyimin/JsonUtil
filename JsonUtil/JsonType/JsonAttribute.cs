using System;

namespace JsonUtil.JsonType
{
    class JsonAttribute : Attribute
    {
        public Type decodec { get; set; }
    }
}
