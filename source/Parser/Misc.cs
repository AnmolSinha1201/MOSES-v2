using System;
using System.Text.RegularExpressions;

namespace MOSESParser
{
	partial class Parser
	{
		string THIS(string code, ref int origin)
		{
			if (code.Length <= origin + "this".Length)
				return null;
			if (!code.Substring(origin, 4).Equals("this", StringComparison.OrdinalIgnoreCase))
				return null;
			origin += 4;
			return "this";
		}
		
		string NAME(string code, ref int origin)
		{
			const string first = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";
			const string last = first + "0123456789";

			int start = origin;
			if (code.Length <= origin + 1)
				return null;
			if (!first.Contains(code[origin].ToString()))
				return null;
			origin++;

			while(code.Length > origin && last.Contains(code[origin].ToString()))
				origin++;
			return code.Substring(start, origin - start);
		}

		string STRING(string code, ref int origin)
		{
			Regex regex = new Regex(@"""(?>[^\\""]+|\\.)*""");
			Match match = regex.Match(code, origin);
			if (match.Success)
			{
				origin = match.Index + match.Length;
				return match.Value;
			}
			return null;
		}

		string INT(string code, ref int origin)
		{
			int start = origin;
			while(origin < code.Length && "0123456789".Contains(code[origin].ToString()))
				origin++;
			if (origin == start)
				return null;
			return code.Substring(start, origin - start);
		}

		string NUMBER(string code, ref int origin)
		{
			return INT(code, ref origin);
		}

		object CRLFWS(string code, ref int origin)
		{
			while (origin < code.Length && "\r\n\t ".Contains(code[origin].ToString()))
				origin++;
			return null;
		}

		object WS(string code, ref int origin)
		{
			while (origin < code.Length && "\t ".Contains(code[origin].ToString()))
				origin++;
			return null;
		}
	}
}