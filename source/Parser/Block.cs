using System;
using System.Text;

namespace MOSESParser
{
	partial class Parser
	{
		public void Test()
		{
			int i = 0;
			Console.WriteLine("test : " + chunk("class asd{\nqwe=123\nasd=456\nqwe(asd, zxc){}}", ref i));
		}

		string chunk(string code, ref int origin)
		{
			StringBuilder sb = new StringBuilder();
			while (true)
			{
				string _block = block(code, ref origin);
				if (_block == null)
					return null;
				sb.Append(sb.Length == 0 ? _block : "\n" + _block);

				if (CRLF(code, ref origin) == null)
					break;
			}
			return sb.ToString();
		}

		/*
		order of parsing :
			functionDeclaration -> innerFuncionBlock (to prevent functions catching)
			includeBlock -> functionDeclaration (to prevent functions catching)
			classDeclaration -> innerFuncionBlock (to prevent name catching)
		*/
		string block(string code, ref int origin)
		{
			return includeBlock(code, ref origin) ?? classDeclaration(code, ref origin) ?? 
			functionDeclaration(code, ref origin) ?? 
			innerFuncionBlock(code, ref origin);
		}

		/*
		order of parsing :
			loops & ifelse -> functions (to prevent functions production catching if/else/loops)
			pre/pos increment/decrement -> variables (to prevent variable production catching variable++)
			functions -> variable (to prevent variable from catching NAME(parameters) of functions)
			tryCatchFinally -> variable (to prevent catching "try" as a variable)
		*/
		string innerFuncionBlock(string code, ref int origin)
		{
			return  prePostIncrementDecrement(code, ref origin) ?? loops(code, ref origin) ?? ifElse(code, ref origin) ??
			tryCatchFinally(code, ref origin) ?? complexFunctionCall(code, ref origin) ?? variableAssign(code, ref origin);
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

		string returnBlock(string code, ref int origin)
		{
			const string _return = "return";
			if (code.Length <= origin + _return.Length)
				return null;
			if (!code.Substring(origin, _return.Length).Equals(_return, StringComparison.OrdinalIgnoreCase
			))
				return null;
			origin += _return.Length;
			string exp = Expression(code, ref origin);
			return _return + exp;
		}

		string innerClassBlock(string code, ref int origin)
        {
            return classDeclaration(code, ref origin) ?? functionDeclaration(code, ref origin) ?? 
            constantVariableAssign(code, ref origin);
        }

		string includeBlock(string code, ref int origin)
		{
			const string __include = "include";
			int pos = origin;
			if (code.Length <= origin + __include.Length)
				return null;
			if (!code.Substring(origin, __include.Length).Equals(__include, StringComparison.OrdinalIgnoreCase))
				return null;
			pos += __include.Length;

			CRLFWS(code, ref pos);
			if (code[pos] != '(')
				return null;
			pos++;
			CRLFWS(code, ref pos);

			string exp = Expression(code, ref pos);
			if (exp == null)
				return null;

			CRLFWS(code, ref pos);
			if (code[pos] != ')')
				return null;
			pos++;
			
			origin = pos;
			return "#include (" + exp + ")";
		}
	}
}