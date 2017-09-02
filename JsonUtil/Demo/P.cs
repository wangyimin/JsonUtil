using JsonUtil.Codecs;
using JsonUtil.JsonType;
using System;
using System.Collections.Generic;

namespace JsonUtil.Demo
{
    class P
    {
        //public List<string> name { get; set; }
        public string[] name { get; set; }
        [JsonAttribute(decodec=typeof(DateTimeImpl))]
        public DateTime birthdate { get; set; }
        //public List<S> s { get; set; }
        public S[] s { get; set; }
        //public S s { get; set; }
        public int age { get; set; }

    }
}
