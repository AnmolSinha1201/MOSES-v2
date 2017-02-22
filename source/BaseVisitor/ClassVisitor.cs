using System.Collections.Generic;
using System.Text;

namespace MOSESParser
{
	public abstract partial class BaseVisitor
	{
        #region classDeclaration
        public class classDeclarationClass
        {
            public string className, classBlock, defaultValue;
            public classDeclarationClass(string className, string classBlock)
            {
                this.className = className;
                this.classBlock = classBlock;
                this.defaultValue = $"class {className}\n{{{classBlock}}}";
            }
        }

		public virtual string classDeclaration(classDeclarationClass context)
        {
            return context.defaultValue;
        }
        #endregion

        #region newInstance
        public class newInstanceClass
        {
            public string className, defaultValue;
            public newInstanceClass(string className)
            {
                this.className = className;
                this.defaultValue = "new " + className;
            }
        }

        public virtual string newInstance(newInstanceClass context)
        {
            return context.defaultValue;
        }
        #endregion

        #region newArray
        public class newArrayClass
        {
            public string defaultValue;
            public List<string> items;
            public newArrayClass(List<string> items)
            {
                this.items = items;

                StringBuilder sb = new StringBuilder();
                foreach (var v in items)
                    sb.Append(sb.Length == 0 ? v : ", " + v);
                this.defaultValue = $"[{sb.ToString()}]";
            }
        }

        public virtual string newArray(newArrayClass context)
        {
            return context.defaultValue;
        }
        #endregion

        #region newDictionary
        public class newDictionaryClass
        {
            public string defaultValue;
            public Dictionary<string, string> items;
            public newDictionaryClass(Dictionary<string, string> items)
            {
                this.items = items;
                
                StringBuilder sb = new StringBuilder();
                foreach (var v in items)
                    sb.Append(sb.Length == 0 ? $"{v.Key} : {v.Value}" : $", {v.Key} : {v.Value}");
                this.defaultValue = $"{{{sb.ToString()}}}";
            }
        }

        public virtual string newDictionary(newDictionaryClass context)
        {
            return context.defaultValue;
        }
        #endregion
	}
}