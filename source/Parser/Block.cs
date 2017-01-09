using System;
using System.Text;

namespace MOSESParser
{
	partial class Parser
	{
		public void Test()
		{
			int i = 0;
			Console.WriteLine("test : " + block("for (1;2;3) asd=10", ref i));
		}

		string chunk(string code, ref int origin)
		{
			while (block(code, ref origin) != null);
			return null;
		}

		string block(string code, ref int origin)
		{
			return innerFuncionBlock(code, ref origin);
		}

		string innerFuncionBlock(string code, ref int origin)
		{
			return  loops(code, ref origin) ?? complexFunctionCall(code, ref origin) ?? variableAssign(code, ref origin);
		}

		string loopBlock(string code, ref int origin)
		{
			string retVal = innerFuncionBlock(code, ref origin);
			if (retVal != null)
				return retVal;

			if (code.Substring(origin, 5).Equals("break"))
			{
				origin += 5;
				return "break";
			}
			else if (code.Substring(origin, 8).Equals("continue"))
			{
				origin += 8;
				return "continue";
			}
			return null;
		}

		string segmentBlock(string code, ref int origin)
		{
			int pos = origin;
			if (code[pos] != '{')
				return innerFuncionBlock(code, ref pos);;
			pos++;

			CRLFWS(code, ref pos);
			StringBuilder blockBuilder = new StringBuilder();
			string blockVal = null;
			while (true)
			{
				CRLFWS(code, ref pos);
				blockVal = innerFuncionBlock(code, ref pos);
				if (blockVal == null)
					break;
				blockBuilder.Append(blockBuilder.Length == 0 ? blockVal : ("\n" + blockVal));
			}

			CRLFWS(code, ref pos);
			if (code[pos] != '}')
				return null;
			pos++;
			origin = pos;
			return blockBuilder.ToString();
		}
	}
}