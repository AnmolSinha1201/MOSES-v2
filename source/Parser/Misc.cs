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

		string DOUBLE(string code, ref int origin)
		{
			int pos = origin;
			string pre = INT(code, ref pos);
			if (pre == null)
				return null;

			if (code.Length <= pos + 1 || code[pos] != '.')
				return null;
			pos++;
			
			string post = INT(code, ref pos);
			if (post == null)
				return null;
			
			origin = pos;
			return pre + "." + post;
		}

		string BINARY(string code, ref int origin)
		{
			int pos = origin;
			if (code.Length <= pos + 2)
				return null;
			if (!code.Substring(pos, 2).Equals("0b", StringComparison.OrdinalIgnoreCase))
				return null;
			pos += 2;
			while(pos < code.Length && "01".Contains(code[pos].ToString()))
				pos++;
			
			string retVal = code.Substring(origin, pos - origin);
			origin = pos;
			return retVal;
		}

		string HEX(string code, ref int origin)
		{
			int pos = origin;
			if (code.Length <= pos + 2)
				return null;
			if (!code.Substring(pos, 2).Equals("0x", StringComparison.OrdinalIgnoreCase))
				return null;
			pos += 2;
			while(pos < code.Length && "0123456789abcdefABCDEF".Contains(code[pos].ToString()))
				pos++;
			
			string retVal = code.Substring(origin, pos - origin);
			origin = pos;
			return retVal;
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

		/*
		order of parsing :
		BINARY/HEX -> INT
		DOUBLE -> INT
		*/
		string NUMBER(string code, ref int origin)
		{
			return BINARY(code, ref origin) ?? HEX(code, ref origin) ?? DOUBLE(code, ref origin) ?? INT(code, ref origin);
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

		string CRLF(string code, ref int origin)
		{
			bool retVal = false;
			while (origin < code.Length && "\r\n".Contains(code[origin].ToString()))
			{
				retVal = true;
				origin++;
			}
			return retVal ? "\n" : null;
		}
	}
}