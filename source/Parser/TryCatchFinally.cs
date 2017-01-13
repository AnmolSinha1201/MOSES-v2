using System;
using System.Text.RegularExpressions;

namespace MOSESParser
{
	partial class Parser
	{
        string tryCatchFinally(string code, ref int origin)
        {
            int pos = origin;
            string __try = _try(code, ref pos);
            if (__try == null)
                return null;
            CRLFWS(code, ref pos);
            string __catch = _catch(code, ref pos);
            CRLFWS(code, ref pos);            
            string __finally = _finally(code, ref pos);
            if (__catch == null && __finally == null)
                return null;
            
            origin = pos;
            return __try + (__catch == null ? "" : " " + __catch) + (__finally == null ? "" : " " +__finally);
        }

        string _try(string code, ref int origin)
        {
            const string __try = "try";
            if (code.Length <= origin + __try.Length)
                return null;

            int pos = origin;
            if (!code.Substring(pos, __try.Length).Equals(__try, StringComparison.OrdinalIgnoreCase))
                return null;
            pos += __try.Length;
            CRLFWS(code, ref pos);
            string segVal = segmentBlock(code, ref pos);
            
            if (segVal == null)
                return null;
            
            origin = pos;
            return __try + " {" + segVal + "}";
        }

        string _catch(string code, ref int origin)
        {
            const string __catch = "catch";
            if (code.Length <= origin + __catch.Length)
                return null;
            
            int pos = origin;
            if (!code.Substring(pos, __catch.Length).Equals(__catch, StringComparison.OrdinalIgnoreCase))
                return null;
            pos += __catch.Length;

            CRLFWS(code, ref pos);
            string outVar = null;
            if (code[pos] == '(')
            {
                pos++;
                CRLFWS(code, ref pos);
                outVar = variable(code, ref pos);
                CRLFWS(code, ref pos);
                if (code[pos] != ')')
                    return null;
                pos++;
                CRLFWS(code, ref pos);               
            }

            string segVal = segmentBlock(code, ref pos);
            if (segVal == null)
                return null;

            origin = pos;
            return __catch + (outVar == null? "" : $"({outVar})") + " {" + segVal + "}";
        }

        string _finally(string code, ref int origin)
        {
            const string __finally = "finally";
            if (code.Length <= origin + __finally.Length)
                return null;

            int pos = origin;
            if (!code.Substring(pos, __finally.Length).Equals(__finally, StringComparison.OrdinalIgnoreCase))
                return null;
            pos += __finally.Length;
            CRLFWS(code, ref pos);
            string segVal = segmentBlock(code, ref pos);
            
            if (segVal == null)
                return null;
            
            origin = pos;
            return __finally + " {" + segVal + "}";
        }
	}
}