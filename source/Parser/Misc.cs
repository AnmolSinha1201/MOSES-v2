using System;
using System.Text.RegularExpressions;

namespace MOSESParser
{
	partial class Parser
	{
		object NAME(string code, ref int origin)
		{
			const string first = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";
			const string last = first + "0123456789";

			int start = origin;
			if (!first.Contains(code[origin].ToString()))
				return null;
			origin++;
			while(last.Contains(code[origin].ToString()))
				origin++;
			return code.Substring(start, origin - start);
		}

		object STRING(string code, ref int origin)
		{
			Regex regex = new Regex(@"""(?>[^\\""]+|\\.)*""");
			Match match = regex.Match(code, origin);
			if (match.Success)
			{
				origin = match.Index;
				return match.Value;
			}
			return null;
		}

		object INT(string code, ref int origin)
		{
			int start = origin;
			while(origin < code.Length && "0123456789".Contains(code[origin].ToString()))
				origin++;
			return code.Substring(start, origin - start);
		}

		object NUMBER(string code, ref int origin)
		{
			return INT(code, ref origin);
		}

		object CRLFWS(string code, ref int origin)
		{
			while ("\r\n\t ".Contains(code[origin].ToString()))
				origin++;
			return null;
		}
	}
}