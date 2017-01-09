using System;
using System.Collections.Generic;

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
			return loopParser(code, ref origin, "loop", 1, ',', false); //delimiter won't be used
		}

		string whileLoop(string code, ref int origin)
		{
			return loopParser(code, ref origin, "while", 1, ',', false); //delimiter won't be used
		}

		string loopParse(string code, ref int origin)
		{
			return loopParser(code, ref origin, "loopParse", 2, ',', false);
		}

		string forLoop(string code, ref int origin)
		{
			return loopParser(code, ref origin, "for", 3, ';', true);
		}

		string loopParser(string code, ref int origin, string loopName, int expCount, char delimiter, bool optionalParams)
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
			return $"{loopName} ({exp.ToString()}) {{{segVal}}}"; 
		}
	}
}