using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ControlEase.Inspec.TreeView
{
    /// <summary>
    /// Class VideoNameService
    /// </summary>
    public class TreeNodeNameService
    {
        /// <summary>
        /// 
        /// </summary>
        protected static Regex mNameIndexRegex = new Regex(@"(?<prefix>\S*)(?<index>[0-9]*)", RegexOptions.RightToLeft | RegexOptions.Compiled);

        /// <summary>
        /// Get the new Name by the old Name
        /// </summary>
        /// <param name="curName">the old Name</param>
        /// <returns>the New name</returns>
        public static string GetNextName(string curName)
        {
            string result = curName + "1";
            Match m = mNameIndexRegex.Match(curName);
            if (m.Success)
            {
                string prefix = m.Groups["prefix"].Value.Trim();
                string index = m.Groups["index"].Value.Trim();

                int len = index.Length;
                if (len >= 15)
                    index = index.Substring(len - 15);

                long num = index == string.Empty ? 0 : Convert.ToInt64(index);
                num++;
                return prefix + num.ToString();
            }
            return result;
        }
    }
}
