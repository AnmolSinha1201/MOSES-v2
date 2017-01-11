using System;
using System.Collections.Generic;
using System.Text;

namespace MOSESParser
{
	partial class Parser
	{
		string functionCall(string code, ref int origin)
		{
			int pos = origin;
			string functionName = NAME(code, ref pos);
			CRLFWS(code, ref pos);
			if (code.Length <= pos + 2)
				return null;
			if (code[pos] != '(')
				return null;
			pos++;

			List<string> expressionList = new List<string>();
			string s;
			CRLFWS(code, ref pos);
			if ((s = Expression(code, ref pos)) != null)
			{
				expressionList.Add(s);
				while (true)
				{
					if (code[pos] != ',')
						break;
					pos++;
					CRLFWS(code, ref pos);
					if ((s = Expression(code, ref pos)) == null){return null;} //error
					expressionList.Add(s);
					CRLFWS(code, ref pos);
				}
			}

			if (code[pos] != ')')
				return null;
			pos++;
			origin = pos;
			
			return functionName + "()";
		}

		string complexFunctionCall(string code, ref int origin)
		{
			string _this = null, vorF = null;
			int pos = origin;
			_this = THIS(code, ref pos);
			vorF = variableOrFunctionChaining(code, ref pos);
			if (vorF != null && vorF[vorF.Length - 1] == ')')
			{
				origin = pos;
				return (_this == null ? "" : "this.") + vorF;
			}
			return null;
		}
	}
}