using System.Collections.Generic;
using System.Text;

namespace MOSESParser
{
	public abstract partial class BaseVisitor
	{
        #region tryCatchFinally
        public class tryCatchFinallyClass
        {
            public string tryBlock, catchBlock, finallyBlock, defaultValue;
            public tryCatchFinallyClass(string tryBlock, string catchBlock, string finallyBlock)
            {
                this.tryBlock = tryBlock;
                this.catchBlock = catchBlock;
                this.finallyBlock = finallyBlock;
                this.defaultValue = tryBlock + (catchBlock == null ? "" : "\n" + catchBlock) + 
                (finallyBlock == null ? "" : "\n" + finallyBlock);
            }
        }

        public virtual string tryCatchFinally(tryCatchFinallyClass context)
        {
            return context.defaultValue;
        }
        #endregion

        #region tryBlock
        public class tryBlockClass
        {
            public string segVal, defaultValue;
            public tryBlockClass(string segVal)
            {
                this.segVal = segVal;
                this.defaultValue = $"try\n{{\n{segVal}\n}}";
            }
        }

        public virtual string tryBlock(tryBlockClass context)
        {
            return context.defaultValue;
        }
        #endregion

        #region catchBlock
        public class catchBlockClass
        {
            public string outVar, segVal, defaultValue;
            public catchBlockClass(string outVar, string segVal)
            {
                this.outVar = outVar;
                this.segVal = segVal;
                this.defaultValue = "catch" + (outVar == null ? "" : $" (outVar)") + $"\n{{\n{segVal}\n}}";
            }
        }

        public virtual string catchBlock(catchBlockClass context)
        {
            return context.defaultValue;
        }
        #endregion

        #region finallyBlock
        public class finallyBlockClass
        {
            public string segVal, defaultValue;
            public finallyBlockClass(string segVal)
            {
                this.segVal = segVal;
                this.defaultValue = $"finally\n{{\n{segVal}\n}}";
            }
        }

        string finallyBlock(finallyBlockClass context)
        {
            return context.defaultValue;
        }
        #endregion
	}
}