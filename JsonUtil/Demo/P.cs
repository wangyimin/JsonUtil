using JsonUtil.Decodecs;
using JsonUtil.JsonType;
using System;

namespace JsonUtil.Demo
{
    class P
    {
        [JsonAttribute(decodec=typeof(DateTimeImpl))]
        public DateTime birthdate { get; set; }
        public string[] name { get; set; }
        public S s { get; set; }
        public int age { get; set; }

    }
}
