using System;
using System.Text;

namespace MOSESParser
{
	partial class Parser
	{
		string variableOrFunctionChaining(string code, ref int origin)
		{
			string retVal =  variableOrFunction(code, ref origin); 
			
			string right = null;
			StringBuilder rightBuilder = new StringBuilder();
			while (true) 
			{
				right = dotUnwrap(code, ref origin) ?? bracketUnwrap(code, ref origin);
				if (right == null)
					break;
				rightBuilder.Append("." + right);
			}

			return retVal + (rightBuilder.Length == 0 ? "" : rightBuilder.ToString());
		}

		string variableOrFunction(string code, ref int origin)
		{
			return functionCall(code, ref origin) ?? variable(code, ref origin);
		}

		string dotUnwrap(string code, ref int origin)
		{
			int pos = origin;
			if (code[pos] != '.')
				return null;
			pos++;
			string retVal = variableOrFunction(code, ref pos);
				
			if (retVal == null)
				return null;
			
			origin = pos;
			return retVal;
		}

		string bracketUnwrap(string code, ref int origin)
		{
			int pos = origin;

			CRLFWS(code, ref pos);
			if (code[pos] != '[')
				return null;
			pos++;
			CRLFWS(code, ref pos);

			string retVal = Expression(code, ref pos);
			if (retVal == null)
				return null;
			
			CRLFWS(code, ref pos);
			if (code[pos] != ']')
				return null;
			pos++;

			origin = pos;
			return retVal;
		}
	}
}