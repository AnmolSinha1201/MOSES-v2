using System;
using System.Collections.Generic;

namespace MOSESParser
{
	partial class Parser
	{
		string binaryOperation(string code, ref int origin)
		{
			return mathematicalOperation(code, ref origin) ?? concatOperation(code, ref origin) ?? 
			logicalOperation(code, ref origin) ?? bitwiseOperation(code, ref origin);
		}
		string mathematicalOperation(string code, ref int origin)
		{
			int pos = origin;
			string exp1 = Expression(code, ref pos);
			if (exp1 == null)
				return null;
			string op;

			CRLFWS(code, ref pos);
			if (code.Length <= pos + 2)
				return null;
			if ((op = opBuilder(code[pos].ToString(), new [] { "**" })) != null)
				pos += 2;
			else if ((op = opBuilder(code[pos].ToString(), new [] { "+", "-", "*", "/", "%" })) != null)
				pos += 1;
			else
				return null;
			CRLFWS(code, ref pos);

			string exp2 = Expression(code, ref pos);
			if (exp2 == null)
				return null;

			origin = pos;
			return exp1 + " " + op + " " + exp2;
		}

		string concatOperation(string code, ref int origin)
		{
			int pos = origin;

			string exp1 = Expression(code, ref pos);
			if (exp1 == null)
				return null;
			
			if (code.Length <= pos)
				return null;
			if (!"\t ".Contains(code[pos].ToString()))
				return null;
			CRLFWS(code, ref pos);
			
			if (code.Length <= pos + 1)
				return null;
			if (code[pos] != '.')
				return null;
			pos++;

			if (code.Length <= pos)
				return null;
			if (!"\t ".Contains(code[pos].ToString()))
				return null;
			CRLFWS(code, ref pos);

			string exp2 = Expression(code, ref pos);
			if (exp2 == null)
				return null;

			origin = pos;
			return exp1 + " . " + exp2;
		}

		string logicalOperation(string code, ref int origin)
		{
			int pos = origin;
			string exp1 = Expression(code, ref pos);
			if (exp1 == null)
				return null;
			string op;

			CRLFWS(code, ref pos);
			if (code.Length <= pos + 2)
				return null;
			if ((op = opBuilder(code[pos].ToString(), new [] { "<=", ">=", "!=", "==", "&&", "||" })) != null)
				pos += 2;
			else if ((op = opBuilder(code[pos].ToString(), new [] { "<", ">" })) != null)
				pos += 1;
			else
				return null;
			CRLFWS(code, ref pos);

			string exp2 = Expression(code, ref pos);
			if (exp2 == null)
				return null;

			origin = pos;
			return exp1 + " " + op + " " + exp2;
		}

		string bitwiseOperation(string code, ref int origin)
		{
			int pos = origin;
			string exp1 = Expression(code, ref pos);
			if (exp1 == null)
				return null;
			string op;

			CRLFWS(code, ref pos);
			if (code.Length <= pos + 2)
				return null;
			if ((op = opBuilder(code[pos].ToString(), new [] { "<<", ">>" })) != null)
				pos += 2;
			else if ((op = opBuilder(code[pos].ToString(), new [] { "&", "|", "^" })) != null)
				pos += 1;
			else
				return null;
			CRLFWS(code, ref pos);

			string exp2 = Expression(code, ref pos);
			if (exp2 == null)
				return null;

			origin = pos;
			return exp1 + " " + op + " " + exp2;
		}
	}
}