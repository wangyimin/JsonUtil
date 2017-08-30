using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace JsonUtil.Utils
{
    class StringUtils
    {
        public static string GetJsonValue(string key, string s)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("Null parameter.");
            }

            MatchCollection mc = new Regex("(?<key>" + key + "\\s*:\\s*)(?<value>.+)").Matches(s);

            string r = mc.Count > 0 ? mc[0].Groups["value"].Value : null;

            if ("\"".Equals(r.Substring(0, 1)))
            {
                mc = Regex.Matches(r, "\"([^\"]*)\"");
                return mc.Count > 0 ? mc[0].ToString() : throw new InvalidOperationException("The number of start/end mark is unmatched.");
            }
            else if ("{".Equals(r.Substring(0, 1)))
            {
                return split(r, '{', '}');
            }
            if ("[".Equals(r.Substring(0, 1)))
            {
                return split(r, '[', ']');
            }
            else
            {
                return r.IndexOf(",") < 0 ? r : r = r.Substring(0, r.IndexOf(","));
            }
        }

        private static string split(string s, char start, char end)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException(nameof(s));
            }

            Stack<string> stack = new Stack<string>();
            stack.Push("*");

            for (int i = 1; i < s.Length; i++)
            {
                char el = s[i];
                if (end.Equals(el))
                {
                    stack.Pop();
                    if (stack.Count == 0)
                    {
                        return s.Substring(1, i - 1);
                    }
                }
                else if (start.Equals(el))
                {
                    stack.Push("*");
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
