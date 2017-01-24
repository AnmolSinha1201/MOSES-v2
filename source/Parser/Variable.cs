using System;

namespace MOSESParser
{
	partial class Parser
	{
		string variable(string code, ref int origin)
		{
			return NAME(code, ref origin);
		}

		string variableAssign(string code, ref int origin)
		{
			string varName = complexVariable(code, ref origin);
			if (varName == null)
				return null;

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
			return varName + " = " + value;
		}

		string opBuilder(string code, string[] matches)
		{
			int len = code.Length, pos;
			if ((pos = Array.IndexOf(matches, code)) >= 0) 
				return matches[pos];
			return null;
		}

		string complexVariable(string code, ref int origin)
		{
			string _this = null, vorF = null;
			int pos = origin;
			_this = THIS(code, ref pos);
			vorF = variableOrFunctionChaining(code, ref pos);
			if (vorF == null || vorF[vorF.Length - 1] == ')')
				return null;

			origin = pos;
			return (_this == null ? "" : "this.") + vorF;
		}

		string constantVariableAssign(string code, ref int origin)
		{
			int pos = origin;
			string varName = variable(code, ref pos);
			if (varName == null)
				return null;
			
			CRLFWS(code, ref pos);
			if (code[pos] != '=')
				return null;
			pos++;
			CRLFWS(code, ref pos);

			string val = constantExpression(code, ref pos);
			if (val == null)
				return null;

			origin = pos;
			return varName + " = " + val;
		}
	}
}