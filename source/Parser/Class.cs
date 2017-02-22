using System;
using System.Collections.Generic;
using System.Text;
using static MOSESParser.BaseVisitor;

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
            return visitor.classDeclaration(new classDeclarationClass(className, classBlockBuilder.ToString()));
        }

        string newInstance(string code, ref int origin)
        {
            int pos = origin;
            const string __new = "new";
            
            if (code.Length <= origin + __new.Length)
                return null;
            if (!code.Substring(origin, __new.Length).Equals(__new, StringComparison.OrdinalIgnoreCase))
                return null;
            pos += __new.Length;

            CRLFWS(code, ref pos);
            string cName = complexFunctionCall(code, ref pos);
            if (cName == null)
                return null;

            origin = pos;
            return visitor.newInstance(new newInstanceClass(cName));
        }

        string newArray(string code, ref int origin)
        {
            int pos = origin;
            if (code[pos] != '[')
                return null;
            pos++;

            CRLFWS(code, ref pos);
            List<string> items = functionCallList(code, ref pos);
            CRLFWS(code, ref pos);

            if (code[pos] != ']')
                return null;
            pos++;

            origin = pos;
            return visitor.newArray(new newArrayClass(items));
        }

        string newDictionary(string code, ref int origin)
        {
            int pos = origin;
            if (code[pos] != '{')
                return null;
            pos++;

            CRLFWS(code, ref pos);
            Dictionary<string, string> dictionaryArgs = dictionaryParamList(code, ref pos);
            CRLFWS(code, ref pos);

            if (code[pos] != '}')
                return null;
            pos++;

            origin = pos;
            return visitor.newDictionary(new newDictionaryClass(dictionaryArgs));
        }

        Dictionary<string, string> dictionaryParamList(string code, ref int origin)
		{
			int pos = origin;
			Dictionary<string, string> expressionList = new Dictionary<string, string>();
            KeyValuePair<string, string>? KVP;
			
			if ((KVP = dictionaryKVP(code, ref pos)) != null && !expressionList.ContainsKey(KVP.Value.Key))
			{
                expressionList.Add(KVP.Value.Key, KVP.Value.Value);
                CRLFWS(code, ref pos);
                while (true)
				{
                    if (code.Length <= ",".Length + pos)
                        break;
					if (code[pos] != ',')
						break;
					pos++;
					CRLFWS(code, ref pos);
					if ((KVP = dictionaryKVP(code, ref pos)) == null || expressionList.ContainsKey(KVP.Value.Key))
                    {return null;} //error

					expressionList.Add(KVP.Value.Key, KVP.Value.Value);
					CRLFWS(code, ref pos);
				}
			}

			origin = pos;
			return expressionList;
		}

        KeyValuePair<string, string>? dictionaryKVP(string code, ref int origin)
        {
            int pos = origin;

            string key = Expression(code, ref pos);
            if (key == null)
                return null;

            CRLFWS(code, ref pos);
            if (code[pos] != ':')
                return null;
            pos++;
            CRLFWS(code, ref pos);

            string value = Expression(code, ref pos);
            if (value == null)
                return null;
            
            origin = pos;
            return new KeyValuePair<string, string>(key, value);
        }
    }
}