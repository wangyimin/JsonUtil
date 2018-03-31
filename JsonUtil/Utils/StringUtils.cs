using System;
using System.Text.RegularExpressions;

namespace JsonUtil.Utils
{
    class StringUtils
    {
        public static string GetJsonValue(string key, ref string s)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("Null parameter.");
            }

            MatchCollection mc = new Regex("(?<key>\"" + key + "\"\\s*:\\s*)(?<value>.+)").Matches(s);
            string part = mc.Count > 0 ? mc[0].Groups["key"].Value : throw new InvalidOperationException("No found specific key[" + key + "]");
            string r = mc[0].Groups["value"].Value;

            if ("\"".Equals(r.Substring(0, 1)))
            {
                mc = Regex.Matches(r, "\"([^\"]*)\"");
                r = mc.Count > 0 ? mc[0].ToString() : throw new InvalidOperationException("The number of start/end mark is unmatched.");
                part = part + r;
            }
            else if ("{".Equals(r.Substring(0, 1)))
            {
                r = split(r, '{', '}');
                part = part + r;
                r = r.Substring(1, r.Length - 2);
            }
            if ("[".Equals(r.Substring(0, 1)))
            {
                r = split(r, '[', ']');
                part = part + r;
                r = r.Substring(1, r.Length - 2);
            }
            else
            {
                r = r.IndexOf(",") < 0 ? r : r = r.Substring(0, r.IndexOf(","));
                part = part + r;
            }

            s = s.Replace(part, "");
            return r;
        }

        private static string split(string s, char start, char end)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException(nameof(s));
            }

            int num = 1;

            for (int i = 1; i < s.Length; i++)
            {
                char el = s[i];
                if (end.Equals(el))
                {
                    num--;
                    if (num == 0)
                    {
                        return s.Substring(0, i + 1);
                    }
                }
                else if (start.Equals(el))
                {
                    num++;
                }
                else
                {
                    //none
                }
            }

            throw new InvalidOperationException("The number of start/end mark is unmatched.");

        }
    }
}