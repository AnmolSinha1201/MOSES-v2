using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MOSESParser
{
    partial class Parser
    {
        string removeComments(string code)
        {
            return removeBlockComment(removeLineComment(code));
        }

        string removeLineComment(string code)
        {            
            StringBuilder sb= new StringBuilder();
            string[] lines = code.Split(new [] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToArray();
            foreach (var line in lines)
            {
                MatchCollection matches = Regex.Matches(line, @"""(?>[^\\\n""]+|\\.)*""|(\/\/.*)");
                string appendVal = line.Substring(0, matches.Count > 0 && matches[matches.Count - 1].Groups[1].Value != "" ?  matches[matches.Count - 1].Groups[1].Index : line.Length);
                
                if (appendVal != "")
                    sb.Append(sb.Length == 0 ? appendVal : "\n" + appendVal);
            }
            
            return sb.ToString();
        }


        string removeBlockComment(string code)
        {
            MatchCollection matches = Regex.Matches(code, @"""(?>[^\\\n""]+|\\.)*""|(\/\*[\S\s]*?\*\/)");
            StringBuilder sb = new StringBuilder(code);
            
            for (int i = matches.Count - 1; i >= 0; i--)
                if (matches[i].Groups[1].Value != "")
                    sb.Remove(matches[i].Index, matches[i].Length);
            return Regex.Replace(sb.ToString(), @"^\s*$[\r\n]*", string.Empty, RegexOptions.Multiline);
        }
    }
}