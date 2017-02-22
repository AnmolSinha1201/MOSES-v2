using System.Collections.Generic;
using System.Text;

namespace MOSESParser
{
	public abstract partial class BaseVisitor
	{
		#region binaryOperation
		public class binaryOperationClass
		{
			public string exp1, exp2, op, defaultValue;
			public binaryOperationClass(string exp1, string op, string exp2)
			{
				this.exp1 = exp1;
				this.exp2 = exp2;
				this.op = op;
				this.defaultValue = $"{exp1} {op} {exp2}";
			}
		}
		
		public virtual string binaryOperation(binaryOperationClass context)
		{
			return context.defaultValue;
		}
		#endregion

		#region ternaryOperation
		public class ternaryIfElseClass
		{
			public string condition, case1, case2, defaultValue;
			public ternaryIfElseClass(string condition, string case1, string case2)
			{
				this.condition = condition;
				this.case1 = case1;
				this.case2 = case2;
				this.defaultValue = $"{condition} ? {case1} : {case2}";
			}
		}

		public virtual string ternaryIfElse(ternaryIfElseClass context)
		{
			return context.defaultValue;
		}
		#endregion

		#region nullCoalascing
		public class nullCoalascingClass
		{
			public string condition, case2, defaultValue;
			public nullCoalascingClass(string condition, string case2)
			{
				this.condition = condition;
				this.case2 = case2;
				this.defaultValue = $"{condition} ?? {case2}";
			}
		}

		public virtual string nullCoalascing(nullCoalascingClass context)
		{
			return context.defaultValue;
		}
		#endregion

	}
}