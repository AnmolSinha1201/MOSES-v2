using System;
using static MOSESParser.BaseVisitor;

namespace MOSESParser
{
	partial class Parser
	{
		string ifElse(string code, ref int origin)
        {
            string _ifBlock = ifBlock(code, ref origin);
            if (_ifBlock == null)
                return null;
            string _elseBlock = elseBlock(code, ref origin);
            return visitor.ifElseBlock(new ifElseBlockClass(_ifBlock, _elseBlock));
        }

        string ifBlock(string code, ref int origin)
        {
            if (code.Length <= origin + "if".Length)
                return null;
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
            return visitor.ifBlock(new ifBlockClass(exp, segVal));
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
            return visitor.elseBlock(new elseBlockClass(segVal));
        }
	}
}