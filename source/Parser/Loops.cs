using System;
using System.Collections.Generic;
using static MOSESParser.BaseVisitor;

namespace MOSESParser
{
	partial class Parser
	{
		string loops(string code, ref int origin)
		{
			return loop(code, ref origin) ?? whileLoop(code, ref origin) ?? 
			loopParse(code, ref origin) ?? forLoop(code, ref origin);
		}

		string loop(string code, ref int origin)
		{
			var v = (loopClass) loopParser(code, ref origin, "loop", 1, ',', false); //delimiter won't be used
			if (v == null)
				return null;
			return visitor.loop(v);
		}

		string whileLoop(string code, ref int origin)
		{
			var v = (whileLoopClass) loopParser(code, ref origin, "while", 1, ',', false); //delimiter won't be used
			if (v == null)
				return null;
			return visitor.whileLoop(v);
		}

		string loopParse(string code, ref int origin)
		{
			var v = (loopParseClass) loopParser(code, ref origin, "loopParse", 2, ',', false);
			if (v == null)
				return null;
			return visitor.loopParse(v);
		}

		string forLoop(string code, ref int origin)
		{
			var v = (forLoopClass) loopParser(code, ref origin, "for", 3, ';', true);
			if (v == null)
				return null;
			return visitor.forLoop(v);
		}

		object loopParser(string code, ref int origin, string loopName, int expCount, char delimiter, bool optionalParams)
		{
			int pos = origin;
			
			if (code.Length <= origin + loopName.Length)
				return null;
			if (!code.Substring(origin, loopName.Length).Equals(loopName, StringComparison.OrdinalIgnoreCase))
				return null;
			pos += loopName.Length;

			CRLFWS(code, ref pos);
			if (code[pos] != '(')
				return null;
			pos++;
			CRLFWS(code, ref pos);

			List<string> exp = new List<string>();
			string temp = Expression(code, ref pos);
			if (temp ==  null && !optionalParams)
				return null;
			exp.Add(temp);
			
			for (int i = 0; i < expCount - 1; i++)
			{
			   	CRLFWS(code, ref pos);
				if (code[pos] != delimiter)
					return null;
				pos++;
				CRLFWS(code, ref pos);

				temp = Expression(code, ref pos);
				if (temp ==  null && !optionalParams)
					return null;
				exp.Add(temp);
			}
			
			CRLFWS(code, ref pos);
			if (code[pos] != ')')
				return null;
			pos++;

			CRLFWS(code, ref pos);
			string segVal = segmentBlock(code, ref pos);
			if (segVal == null)
				return null;
			origin = pos;
			return loopClassConstructor(loopName, exp, segVal); 
		}

		object loopClassConstructor(string loopName, List<string> exp, string segVal)
		{
			switch (loopName)
			{
				case "loop" :
				return new loopClass(exp[0], segVal);
				break;
				
				case "loopParse" :
				return new loopParseClass(exp[0], exp[1], segVal);
				break;

				case "while" :
				return new whileLoopClass(exp[0], segVal);
				break;

				case "for" :
				return new forLoopClass(exp[0], exp[1], exp[2],  segVal);
				break;
			}
			return null;
		}
	}
}