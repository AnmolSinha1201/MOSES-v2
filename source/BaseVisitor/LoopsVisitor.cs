using System.Collections.Generic;
using System.Text;

namespace MOSESParser
{
	public abstract partial class BaseVisitor
	{
        #region loop
        public class loopClass
        {
            public string iterations, segVal, defaultValue;
            public loopClass(string iterations, string segVal)
            {
                this.iterations = iterations;
                this.segVal = segVal;
                this.defaultValue = $"loop ({iterations})\n{{\n{segVal}\n}}";
            }
        }

		public virtual string loop(loopClass context)
		{
            return context.defaultValue;
		}
        #endregion

        #region whileLoop
        public class whileLoopClass
        {
            public string condition, segVal, defaultValue;
            public whileLoopClass(string condition, string segVal)
            {
                this.condition = condition;
                this.segVal = segVal;
                this.defaultValue = $"while ({condition})\n{{\n{segVal}\n}}";
            }
        }

		public virtual string whileLoop(whileLoopClass context)
		{
            return context.defaultValue;
		}
        #endregion

        #region loopParse
        public class loopParseClass
        {
            public string input, delimiter, segVal, defaultValue;
            public loopParseClass(string input, string delimiter, string segVal)
            {
                this.input = input;
                this.delimiter = delimiter;
                this.segVal = segVal;
                this.defaultValue = $"loopParse ({input}, {delimiter})\n{{\n{segVal}\n}}";
            }
        }

		public virtual string loopParse(loopParseClass context)
		{
            return context.defaultValue;
		}
        #endregion

        #region forLoop
        public class forLoopClass
        {
            public string initial, condition, increment, segVal, defaultValue;
            public forLoopClass(string initial, string condition, string increment, string segVal)
            {
                this.initial = initial;
                this.condition = condition;
                this.increment = increment;
                this.segVal = segVal;
                this.defaultValue = $"for ({initial}; {condition}; {increment})\n{{\n{segVal}\n}}";
            }
        }

		public virtual string forLoop(forLoopClass context)
		{
            return context.defaultValue;
		}
        #endregion
	}
}