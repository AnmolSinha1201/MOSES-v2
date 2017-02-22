using System;
using System.Text;
using static MOSESParser.BaseVisitor;

namespace MOSESParser
{
	partial class Parser
	{
		public void Test()
		{
			int i = 0;
			Console.WriteLine("test : " + chunk("for(3; 123; 123){qwe=[10,20]\nasd=5\nbreak\nreturn, 123}", ref i));
		}

		public BaseVisitor visitor;
		string chunk(string code, ref int origin)
		{
			code = removeComments(code);
			StringBuilder sb = new StringBuilder();
			while (true)
			{
				WS(code, ref origin);
				string _block = block(code, ref origin);
				WS(code, ref origin);
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
			tryCatchFinally(code, ref origin) ?? returnBlock(code, ref origin) ?? complexFunctionCall(code, ref origin) ?? variableAssign(code, ref origin);
		}

		string loopBlock(string code, ref int origin)
		{
			string retVal = innerFuncionBlock(code, ref origin);
			if (retVal != null)
				return retVal;

			if (code.Length >= origin + "break".Length && code.Substring(origin, 5).Equals("break"))
			{
				origin += 5;
				return "break;";
			}
			else if (code.Length >= origin + "continue".Length && code.Substring(origin, 8).Equals("continue"))
			{
				origin += 8;
				return "continue;";
			}
			return null;
		}

		string segmentBlock(string code, ref int origin)
		{
			int pos = origin;
			if (code[pos] != '{')
				return loopBlock(code, ref pos);;
			pos++;

			CRLFWS(code, ref pos);
			StringBuilder blockBuilder = new StringBuilder();
			string blockVal = null;
			while (true)
			{
				CRLFWS(code, ref pos);
				blockVal = loopBlock(code, ref pos);
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
			int pos = origin;
			if (code.Length < pos + _return.Length)
				return null;
			if (!code.Substring(pos, _return.Length).Equals(_return, StringComparison.OrdinalIgnoreCase
			))
				return null;
			pos += _return.Length;

			bool bComma = false;

			WS(code, ref pos);
			if (code.Length < pos + 1)
			{
				origin = pos;
				return _return;
			}
			
			if (code[pos] == ',')
				bComma = true;
			pos++;

			WS(code, ref pos);
			string exp = Expression(code, ref pos);
			if (bComma && exp == null)
				return null;
			
			origin = pos;
			return visitor.returnBlock(new returnBlockClass(exp));
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
			return visitor.includeBlock(new includeBlockClass(exp));
		}
	}
}