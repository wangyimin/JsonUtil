using JsonUtil.Base;
using System;

namespace JsonUtil.Codecs
{
    class DateTimeImpl : Decodec
    {
        public string Convert<DateTime>(DateTime obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return "\"" + obj + "\"";
        }
    }
}
