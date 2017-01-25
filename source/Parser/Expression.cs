namespace MOSESParser
{
	partial class Parser
	{
		/*
		order of parsing :
		newInstance -> complexVariable
		complexFunctionCall -> complexVariable
		variableAssign -> complexVariable
		binary/ternary -> before everything else (since everything can be evaluated)
		*/
		string Expression(string code, ref int origin)
		{
			string part1 = constantExpression(code, ref origin) ?? 
			newInstance(code, ref origin) ?? newDictionary(code, ref origin) ?? newArray(code, ref origin) ?? 
			variableAssign(code, ref origin)?? complexFunctionCall(code, ref origin) ?? complexVariable(code, ref origin) ??
			unaryOperation(code, ref origin) ?? prePostIncrementDecrement(code, ref origin);

			CRLFWS(code, ref origin);
			string part2 = ternaryOperation(code, ref origin, part1);
			return part2 == null ? part1 : part2;
		}

		string constantExpression(string code, ref int origin)
		{
			return STRING(code, ref origin) ?? NUMBER(code, ref origin);
		}
	}
}