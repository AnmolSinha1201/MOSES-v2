using System.Collections.Generic;
using System.Text;

namespace MOSESParser
{
	public abstract partial class BaseVisitor
	{
        #region STRING
        public class STRINGClass
        {
            public string stringValue, defaultValue;
            public STRINGClass(string stringValue)
            {
                this.stringValue = stringValue;
                this.defaultValue = stringValue;
            }
        }

		public virtual string STRING(STRINGClass context)
		{
            return context.defaultValue;
		}
        #endregion
	}
}