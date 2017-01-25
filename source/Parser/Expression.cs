namespace MOSESParser
{
	partial class Parser
	{
		/*
		order of parsing :
		newInstance -> complexVariable
		complexFunctionCall -> complexVariable
		variableAssign -> complexVariable
		binary/ternary -> A->B(.A)?
		*/
		string Expression(string code, ref int origin)
		{
			string part1 = constantExpression(code, ref origin) ?? 
			newInstance(code, ref origin) ?? newDictionary(code, ref origin) ?? newArray(code, ref origin) ?? 
			variableAssign(code, ref origin)?? complexFunctionCall(code, ref origin) ?? complexVariable(code, ref origin) ??
			unaryOperation(code, ref origin) ?? prePostIncrementDecrement(code, ref origin) ??
			priorityExpression(code, ref origin);

			if (part1 == null)
				return null;
			CRLFWS(code, ref origin);
			
			string part2 = ternaryOperation(code, ref origin, part1) ?? binaryOperation(code, ref origin, part1);
			return part2 == null ? part1 : part2;
		}

		string constantExpression(string code, ref int origin)
		{
			return STRING(code, ref origin) ?? NUMBER(code, ref origin);
		}

		string priorityExpression(string code, ref int origin)
		{
			int pos = origin;
			if (code.Length <= pos || code[pos] != '(')
				return null;
			pos++;

			CRLFWS(code, ref pos);
			string exp = Expression(code, ref pos);
			CRLFWS(code, ref pos);

			if (code.Length <= pos || code[pos] != ')')
				return null;
			pos++;

			origin = pos;
			return "( " + exp + " )";
		}
	}
}