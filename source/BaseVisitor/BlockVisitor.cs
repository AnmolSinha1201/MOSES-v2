using System.Collections.Generic;
using System.Text;

namespace MOSESParser
{
	public abstract partial class BaseVisitor
	{
		#region returnBlock
		public class returnBlockClass
		{
			public string exp, defaultValue;
			public returnBlockClass(string exp)
			{
				this.exp = exp;
				this.defaultValue = "return" + (exp == null ? "" : " " + exp) + ";";
			}
		}

		public virtual string returnBlock(returnBlockClass context)
		{
			return context.defaultValue;
		}
		#endregion
		
		#region includeBlock
		public class includeBlockClass
		{
			public string fileName, defaultValue;
			public includeBlockClass(string fileName)
			{
				this.fileName = fileName;
				this.defaultValue = $"include ({fileName})";
			}
		}

		public virtual string includeBlock(includeBlockClass context)
		{
			return context.defaultValue;
		}
		#endregion
	}
}