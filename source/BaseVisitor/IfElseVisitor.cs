using System.Collections.Generic;
using System.Text;

namespace MOSESParser
{
	public abstract partial class BaseVisitor
	{
        #region ifElseBlock
        public class ifElseBlockClass
        {
            public string ifBlock, elseBlock, defaultValue;
            public ifElseBlockClass(string ifBlock, string elseBlock)
            {
                this.ifBlock = ifBlock;
                this.elseBlock = elseBlock;
                this.defaultValue = ifBlock + (elseBlock == null ? "" : "\n" + elseBlock);
            }
        }

        public virtual string ifElseBlock(ifElseBlockClass context)
        {
            return context.defaultValue;
        }
        #endregion

        #region ifBlock
        public class ifBlockClass
        {
            public string condition, segVal, defaultValue;
            public ifBlockClass(string condition, string segVal)
            {
                this.condition = condition;
                this.segVal = segVal;
                this.defaultValue = $"if ({condition})\n{{\n{segVal}\n}}";
            }
        }

        public virtual string ifBlock(ifBlockClass context)
        {
            return context.defaultValue;
        }
        #endregion

        #region elseBlock
        public class elseBlockClass
        {
            public string segVal, defaultValue;
            public elseBlockClass(string segVal)
            {
                this.segVal = segVal;
                this.defaultValue = $"else\n{{\n{segVal}\n}}";
            }
        }

        public virtual string elseBlock(elseBlockClass context)
        {
            return context.defaultValue;
        }
        #endregion
	}
}