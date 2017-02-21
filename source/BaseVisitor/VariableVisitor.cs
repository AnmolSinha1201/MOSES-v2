namespace MOSESParser
{
	public abstract partial class BaseVisitor
	{
        #region variable
        public class variableClass
        {
            public string variableName, defaultValue;
            public variableClass(string variableName)
            {
                this.variableName = variableName;
                this.defaultValue = variableName;
            }
        }

        public virtual string variable(variableClass context)
		{
			return context.defaultValue;
		}
        #endregion

        #region variableAssign
		public class variableAssignClass
		{
			public string variableName, value, op, defaultValue;
			public variableAssignClass(string variableName, string op, string value)
			{
				this.variableName = variableName;
				this.op = op;
				this.value = value;
                this.defaultValue = $"{variableName} {op} {value}";
			}
		}

        public virtual string variableAssign(variableAssignClass context)
        {
            return context.defaultValue;
        }
        #endregion
        
        #region complexVariable
        public class complexVariableClass
        {
            public string _this, variableOrFunctionChaining, defaultValue;
            public complexVariableClass(string _this, string variableOrFunctionChaining)
            {
                this._this = _this;
                this.variableOrFunctionChaining = variableOrFunctionChaining;
                this.defaultValue = (string.IsNullOrEmpty(_this)? "" : "this.") + variableOrFunctionChaining;
            }
        }

		public virtual string complexVariable(complexVariableClass context)
		{
			return context.defaultValue;
		}
        #endregion

        #region constantVariableAssign
        public class constantVariableAssignClass
        {
            public string variableName, constantExpression, defaultValue;
            public constantVariableAssignClass(string variableName, string constantExpression)
            {
                this.variableName = variableName;
                this.constantExpression = constantExpression;
                this.defaultValue = $"{variableName} = {constantExpression}";
            }
        }

		public virtual string constantVariableAssign(constantVariableAssignClass context)
		{
            return context.defaultValue;
		}
        #endregion
	}
}