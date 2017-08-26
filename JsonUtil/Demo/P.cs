using JsonUtil.Codecs;
using JsonUtil.JsonType;
using System;

namespace JsonUtil.Demo
{
    class P
    {
        public S[] s { get; set; }
        [JsonAttribute(decodec=typeof(DateTimeImpl))]
        public DateTime birthdate { get; set; }
        public string[] name { get; set; }
        //public S s { get; set; }
        public int age { get; set; }

    }
}
