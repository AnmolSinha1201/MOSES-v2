using System;
using System.Collections.Generic;

namespace MOSESParser
{
	partial class Parser
	{
        string prePostIncrementDecrement(string code, ref int origin)
        {
            return preIncrementDecrement(code, ref origin) ?? postIncrementDecrement(code, ref origin);
        }

        string preIncrementDecrement(string code, ref int origin)
        {
            int pos = origin;
            if (code.Length <= origin + 2)
                return null;

            string op = code.Substring(pos, 2);
            if (op != "++" && op != "--")
                return null;
            pos += 2;

            string var = complexVariable(code, ref pos);
            if (var == null)
                return null;

            origin = pos;
            return op + var;
        }

        string postIncrementDecrement(string code, ref int origin)
        {
            int pos = origin;
            if (code.Length <= origin + 2)
                return null;

            string var = complexVariable(code, ref pos);
            if (var == null)
                return null;

            string op = code.Substring(pos, 2);
            if (op != "++" && op != "--")
                return null;
            pos += 2;
            
            origin = pos;
            return var + op;
        }
	}
}