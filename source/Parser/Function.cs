using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOSESParser
{
	partial class Parser
	{
		string functionCall(string code, ref int origin)
		{
			int pos = origin;
			string functionName = NAME(code, ref pos);
			CRLFWS(code, ref pos);
			if (code.Length <= pos + 1)
				return null;
			if (code[pos] != '(')
				return null;
			pos++;

			CRLFWS(code, ref pos);
			List<string> expressionList = functionCallList(code, ref pos);

			if (code.Length <= pos + 1)
				return null;
			if (code[pos] != ')')
				return null;
			pos++;

			origin = pos;
			return functionName + "("+ expressionList.Count +")";
		}

		List<string> functionCallList(string code, ref int origin)
		{
			int pos = origin;
			List<string> expressionList = new List<string>();
			string s;
			
			if ((s = Expression(code, ref pos)) != null)
			{
				expressionList.Add(s);
				CRLFWS(code, ref pos);
				while (true)
				{
					if (code[pos] != ',')
						break;
					pos++;
					CRLFWS(code, ref pos);
					if ((s = Expression(code, ref pos)) == null){return null;} //error
					expressionList.Add(s);
					CRLFWS(code, ref pos);
				}
			}

			origin = pos;
			return expressionList;
		}

		string complexFunctionCall(string code, ref int origin)
		{
			string _this = null, vorF = null;
			int pos = origin;
			_this = THIS(code, ref pos);
			vorF = variableOrFunctionChaining(code, ref pos);
			if (vorF != null && vorF[vorF.Length - 1] == ')')
			{
				origin = pos;
				return (_this == null ? "" : "this.") + vorF;
			}
			return null;
		}

		string functionDeclaration(string code, ref int origin)
		{
			string fDef = functionDefinition(code, ref origin);
			if (fDef == null)
				return null;
			
			string fBody = functionBody(code, ref origin);
			if (fBody == null)
				return null;
			
			return fDef + " " + fBody;
		}

		string functionDefinition(string code, ref int origin)
		{
			int pos = origin;
			string functionName = NAME(code, ref pos);
			if (functionName == null)
				return null;
			CRLFWS(code, ref pos);
			
			if (code[pos] != '(')
				return null;
			pos++;

			CRLFWS(code, ref pos);
			List<functionParameter> FPList = functionParameterList(code, ref pos);
			CRLFWS(code, ref pos);

			if (code[pos] != ')')
				return null;
			pos++;

			origin = pos;
			return $"{functionName} ({FPList.Count})";
		}

		string functionBody(string code, ref int origin)
		{
			int pos = origin;
			if (code[pos] != '{')
				return null;
			pos++;
			CRLFWS(code, ref pos);

			string segVal;
			StringBuilder segValBlock = new StringBuilder();
			while((segVal = innerFuncionBlock(code, ref pos)) != null)
			{
				segValBlock.Append(segVal);
				CRLFWS(code, ref pos);
			}

			if (code[pos] != '}')
				return null;
			pos++;
			
			origin = pos;
			return "{ " + segValBlock.ToString() + " }";
		}

		class functionParameter
		{
			public string defaultValue, parameterName;
			public bool byRef, variadic;
		}

		#region functionParameterList
		
		List<functionParameter> functionParameterList(string code, ref int origin)
		{
			List<functionParameter> FPList = new List<functionParameter>();
			List<functionParameter> FP;

			if ((FP = parameterListType1(code, ref origin)) != null)
				FPList.AddRange(FP);
			if ((FP = parameterListType2(code, ref origin)) != null)
				FPList.AddRange(FP);
			if ((FP = parameterListType3(code, ref origin)) != null)
				FPList.AddRange(FP);
			return FPList;
		}

		/*
		NoDefault can consume others as well, therefore check if string can be consumed by Default or 
		Variadic procuctions first. If they can be consumed, then return as the parameters belong to
		other type of list.
		*/
		List<functionParameter> parameterListType1(string code, ref int origin)
		{
			int pos = origin;
			functionParameter FPNoDefault;
			List<functionParameter> FPList = new List<functionParameter>();

			if (functionParameterDefault(code, ref pos) != null || functionParameterVariadic(code, ref pos) != null ||(FPNoDefault = functionParameterNoDefault(code, ref pos)) == null)
				return null;
			FPList.Add(FPNoDefault);
			
			CRLFWS(code, ref pos);
			while (true)
			{
				int pos2 = pos;
				if (code[pos2] != ',')
					break;
				pos2++;
				CRLFWS(code, ref pos2);
				if ((functionParameterDefault(code, ref pos2) != null || functionParameterVariadic(code, ref pos2) != null) ||
					(FPNoDefault = functionParameterNoDefault(code, ref pos2)) == null)
					break;
				pos = pos2;
				FPList.Add(FPNoDefault);
				CRLFWS(code, ref pos);
			}
			
			origin = pos;
			return FPList.Concat(parameterListType2B(code, ref origin)).ToList();
		}

		List<functionParameter> parameterListType2(string code, ref int origin)
		{
			functionParameter FPDefault;
			List<functionParameter> FPList = new List<functionParameter>();
			if ((FPDefault = functionParameterDefault(code, ref origin)) == null)
				return null;
			FPList.Add(FPDefault);
			
			CRLFWS(code, ref origin);			
			return FPList.Concat(parameterListType2B(code, ref origin)).ToList();
		}

		List<functionParameter> parameterListType3(string code, ref int origin)
		{
			functionParameter FPVariadic;
			List<functionParameter> FPList = new List<functionParameter>();
			if ((FPVariadic = functionParameterVariadic(code, ref origin)) == null)
				return null;
			FPList.Add(FPVariadic);
			
			CRLFWS(code, ref origin);			
			return FPList;
		}

		List<functionParameter> parameterListType2B(string code, ref int origin)
		{
			int pos = origin;
			functionParameter FPDefault, FPVariadic;
			List<functionParameter> FPList = new List<functionParameter>();

			while (true)
			{
				int pos2 = pos;
				if (code[pos2] != ',')
					break;
				pos2++;
				CRLFWS(code, ref pos2);
				if ((FPDefault = functionParameterDefault(code, ref pos2)) == null)
					break;
				pos = pos2;
				FPList.Add(FPDefault);
				CRLFWS(code, ref pos);
			}

			while (true)
			{
				int pos2 = pos;
				if (code[pos2] != ',')
					break;
				pos2++;
				CRLFWS(code, ref pos2);
				if ((FPVariadic = functionParameterVariadic(code, ref pos2)) == null)
					break;
				pos = pos2;
				FPList.Add(FPVariadic);
				CRLFWS(code, ref pos);
			}

			origin = pos;
			return FPList;
		}

		functionParameter functionParameterDefault(string code, ref int origin)
		{
			int pos = origin;
			bool __ref = isRef(code, ref pos) != null;

			CRLFWS(code, ref pos);
			string varName = NAME(code, ref pos);
			if (varName == null)
				return null;
			CRLFWS(code, ref pos);

			if (code[pos] != '=')
				return null;
			pos++;
			CRLFWS(code, ref pos);

			string exp = constantExpression(code, ref pos);
			if (exp == null)
				return null;
			
			origin = pos;
			return new functionParameter() { defaultValue = exp, byRef = __ref, variadic = false, parameterName = varName };
		}

		functionParameter functionParameterNoDefault(string code, ref int origin)
		{
			int pos = origin;
			bool __ref = isRef(code, ref pos) != null;

			CRLFWS(code, ref pos);
			string varName = NAME(code, ref pos);
			if (varName == null)
				return null;
			
			origin = pos;
			return new functionParameter() { defaultValue = null, byRef = __ref, variadic = false, parameterName = varName };
		}

		functionParameter functionParameterVariadic(string code, ref int origin)
		{
			int pos = origin;
			string varName = NAME(code, ref pos);
			if (varName == null)
				return null;
			
			CRLFWS(code, ref pos);
			if (code[pos] != '*')
				return null;
			pos++;
			
			origin = pos;
			return new functionParameter() { defaultValue = null, byRef = false, variadic = true, parameterName = varName };
		}

		string isRef(string code, ref int origin)
		{
			const string __ref = "ref";
			if (code.Length <= origin + __ref.Length)
				return null;
			if (!code.Substring(origin, __ref.Length).Equals(__ref, StringComparison.CurrentCultureIgnoreCase))
				return null;
			
			origin += __ref.Length;
			return __ref;
		}

		#endregion
	}
}