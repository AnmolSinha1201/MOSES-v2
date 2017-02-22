using static MOSESParser.BaseVisitor;

namespace MOSESParser
{
	partial class Parser
	{
		string ternaryOperation(string code, ref int origin, string preExpression)
		{
			return nullCoalascing(code, ref origin, preExpression) ?? ternaryIfElse(code, ref origin, preExpression);
		}

		string ternaryIfElse(string code, ref int origin, string preExpression)
		{
			int pos = origin;
			string exp1 = preExpression;
			if (code.Length < pos + 1 || code[pos] != '?')
				return null;
			pos++;
			string exp2, exp3;

			CRLFWS(code, ref pos);
			if ((exp2 = Expression(code, ref pos)) == null)
				return null;

			CRLFWS(code, ref pos);
			if (code.Length < pos + 1 || code[pos] != ':')
				return null;
			pos++;

			CRLFWS(code, ref pos);
			if ((exp3 = Expression(code, ref pos)) == null)
				return null;

			origin = pos;
			return visitor.ternaryIfElse(new ternaryIfElseClass(exp1, exp2, exp3));
		}

		string nullCoalascing(string code, ref int origin, string preExpression)
		{
			int pos = origin;
			string exp1 = preExpression;
			if (code.Length < pos + "??".Length || code.Substring(pos, 2) != "??")
				return null;
			pos += 2;
			string exp2;

			CRLFWS(code, ref pos);
			if ((exp2 = Expression(code, ref pos)) == null)
				return null;

			origin = pos;
			return visitor.nullCoalascing(new nullCoalascingClass(exp1, exp2));
		}
	}
}