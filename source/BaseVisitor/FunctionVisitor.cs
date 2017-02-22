using System.Collections.Generic;
using System.Text;

namespace MOSESParser
{
	public abstract partial class BaseVisitor
	{
        #region functionCall
        public class functionCallClass
        {
            public string functionName, defaultValue;
            List<string> functionCallList;
            public functionCallClass(string functionName, List<string> functionCallList)
            {
                this.functionName = functionName;
                this.functionCallList = functionCallList;

                StringBuilder sb = new StringBuilder();
                foreach (var v in functionCallList)
                    sb.Append(sb.Length == 0 ? v : ", " + v);
                this.defaultValue = $"{functionName} ({sb.ToString()})";
            }
        }

        public virtual string functionCall(functionCallClass context)
		{
            return context.defaultValue;
		}
        #endregion

        #region  complexFunctionCall
        public class complexFunctionCallClass
        {
            public string _this, variableOrFunctionChaining, defaultValue;
            public complexFunctionCallClass(string _this, string variableOrFunctionChaining)
            {
                this._this = _this;
                this.variableOrFunctionChaining = variableOrFunctionChaining;
                this.defaultValue = (string.IsNullOrEmpty(_this) ? "" : "this.") + variableOrFunctionChaining;
            }
        }

		public virtual string complexFunctionCall(complexFunctionCallClass context)
		{
			return context.defaultValue;
		}
        #endregion

        #region functionDeclaration
        public class functionDeclarationClass
        {
            public string functionDefinition, functionBody, defaultValue;
            public functionDeclarationClass(string functionDefinition, string functionBody)
            {
                this.functionDefinition = functionDefinition;
                this.functionBody = functionBody;
                this.defaultValue = $"{functionDefinition}\n{{{functionBody}}}";
            }
        }

		public virtual string functionDeclaration(functionDeclarationClass context)
		{
			return context.defaultValue;
		}
        #endregion

        #region functionDefinition
        public class functionDefinitionClass
        {
            public string functionName, defaultValue;
            public List<functionParameter> functionParameterList;
            public functionDefinitionClass(string functionName, List<functionParameter> functionParameterList)
            {
                this.functionName = functionName;
                this.functionParameterList = functionParameterList;

                StringBuilder sb = new StringBuilder();
                foreach (var v in functionParameterList)
                    sb.Append(sb.Length == 0 ? v.ToString() : ", " + v.ToString());
                this.defaultValue = $"{functionName} ({sb.ToString()})";
            }
        }

		public virtual string functionDefinition(functionDefinitionClass context)
		{
            return context.defaultValue;
		}
        #endregion

        #region functionBody
        public class functionBodyClass
        {
            public string innerFuncionBlock, defaultValue;
            public functionBodyClass(string innerFuncionBlock)
            {
                this.innerFuncionBlock = innerFuncionBlock;
                this.defaultValue = innerFuncionBlock;
            }
        }

		public virtual string functionBody(functionBodyClass context)
		{
            return context.defaultValue;
		}
        #endregion

		public class functionParameter
		{
			public string defaultValue, parameterName;
			public bool byRef, variadic;

            override public string ToString()
            {
                return (byRef ? "byRef " : "") + parameterName + 
                (variadic ? "*" : "" ) +
                (string.IsNullOrEmpty(defaultValue) ? " = " + defaultValue : "");
            }
		}

	}
}