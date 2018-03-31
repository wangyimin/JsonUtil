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
        public static string GetJsonValue(string key, ref string s)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("Null parameter.");
            }

            MatchCollection mc = new Regex("(?<key>\"" + key + "\"\\s*:\\s*)(?<value>.+)").Matches(s);

            string part = mc.Count > 0 ? mc[0].Groups["value"].Value : null;

            if ("\"".Equals(part.Substring(0, 1)))
            {
                mc = Regex.Matches(part, "\"([^\"]*)\"");
                return mc.Count > 0 ? mc[0].ToString() : throw new InvalidOperationException("The number of start/end mark is unmatched.");
            }
            else if ("{".Equals(part.Substring(0, 1)))
            {
                string r = split(part, '{', '}');
                s = new Regex("\"" + Regex.Escape(key) + "\"\\s*:\\s*" + Regex.Escape("{" + r + "}") + "\\,").Replace(s, "");
                return r;
            }
            if ("[".Equals(part.Substring(0, 1)))
            {
                string r = split(part, '[', ']');
                s = new Regex("\"" + Regex.Escape(key) + "\"\\s*:\\s*" + Regex.Escape("[" + r + "]") + "\\,").Replace(s, "");
                return r;
            }
            else
            {
                return part.IndexOf(",") < 0 ? part : part = part.Substring(0, part.IndexOf(","));
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
