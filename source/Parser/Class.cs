using System;
using System.Text;

namespace MOSESParser
{
    partial class Parser
    {
        string classDeclaration(string code, ref int origin)
        {
            const string __class = "class";
            int pos = origin;
            if (code.Length <= origin + __class.Length)
                return null;
            if (!code.Substring(pos, __class.Length).Equals(__class, StringComparison.OrdinalIgnoreCase))
                return null;
            pos += __class.Length;
            CRLFWS(code, ref pos);

            string className = NAME(code, ref pos);
            if (className == null)
                return null;
            CRLFWS(code, ref pos);

            if (code[pos] != '{')
                return null;
            pos++;
            CRLFWS(code, ref pos);

            string classVal;
            StringBuilder classBlockBuilder = new StringBuilder();
            while ((classVal = innerClassBlock(code, ref pos)) != null)
            {
                classBlockBuilder.Append(classVal);
                CRLFWS(code, ref pos);
            }

            if (code[pos] != '}')
                return null;
            pos++;

            origin = pos;
            return $"class {className} {{ {classBlockBuilder.ToString()} }} ";
        }
    }
}