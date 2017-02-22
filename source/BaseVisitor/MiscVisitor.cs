using System.Collections.Generic;
using System.Text;

namespace MOSESParser
{
	public abstract partial class BaseVisitor
	{
        #region STRING
        public class stringClass
        {
            public string stringValue, defaultValue;
            public stringClass(string stringValue)
            {
                this.stringValue = stringValue;
                this.defaultValue = stringValue;
            }
        }

		public virtual string STRING(stringClass context)
		{
            return context.defaultValue;
		}
        #endregion
	}
}