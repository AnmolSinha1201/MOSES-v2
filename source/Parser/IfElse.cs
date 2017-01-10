using System;
using System.Text.RegularExpressions;

namespace MOSESParser
{
	partial class Parser
	{
		string ifElse(string code, ref int origin)
        {
            string _ifBlock = ifBlock(code, ref origin);
            if (_ifBlock == null)
                return null;
            return _ifBlock + elseBlock(code, ref origin);
        }

        string ifBlock(string code, ref int origin)
        {
            int pos = origin;
            if (!code.Substring(pos, 2).Equals("if", StringComparison.OrdinalIgnoreCase))
                return null;
            pos += 2;
            
            CRLFWS(code, ref pos);
            if (code[pos] != '(')
                return null;
            pos++;
            CRLFWS(code, ref pos);

            string exp = Expression(code, ref pos);

            CRLFWS(code, ref pos);
            if (code[pos] != ')')
                return null;
            pos++;
            CRLFWS(code, ref pos);
            
            string segVal = segmentBlock(code, ref pos);
            if (segVal == null)
                return null;
            CRLFWS(code, ref pos);

            origin = pos;
            return $"if ({exp}) {{{segVal}}}";
        }

        string elseBlock(string code, ref int origin)
        {
            int pos = origin;
            if (code.Length <= origin + "else".Length)
                return null;
            if (!code.Substring(pos, 4).Equals("else", StringComparison.OrdinalIgnoreCase))
                return null;
            pos += 4;

            CRLFWS(code, ref pos);
            string segVal = segmentBlock(code, ref pos);
            if (segVal == null)
                return null;
            CRLFWS(code, ref pos);

            origin = pos;
            return $"else {{{segVal}}}";
        }
	}
}