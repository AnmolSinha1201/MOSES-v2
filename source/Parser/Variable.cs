using System;

namespace MOSESParser
{
    partial class Parser
    {
        object variable(string code, ref int origin)
        {
            return NAME(code, ref origin);
        }

        object variableAssign(string code, ref int origin)
        {
            string varName = (string)variable(code, ref origin);

            CRLFWS(code, ref origin);
            string op = null;
            if ((op = opBuilder(code[origin].ToString(), new [] {"="})) != null)
                origin++;
            else if ((op = opBuilder(code.Substring(origin, 2), new [] { "+=", "-=", "*=", "/=", "%=", ".=", "|=", "&=", "^=" })) != null)
                origin += 2;
            else if ((op = opBuilder(code.Substring(origin, 3), new [] { "**=", ">>=", "<<=" })) != null)
                origin += 3;
            else
                return null;
            CRLFWS(code, ref origin);

            string value = (string)Expression(code, ref origin);
            return true;
        }

        string opBuilder(string code, string[] matches)
        {
            int len = code.Length, pos;
            if ((pos = Array.IndexOf(matches, code)) >= 0) 
                return matches[pos];
            return null;
        }
    }
}