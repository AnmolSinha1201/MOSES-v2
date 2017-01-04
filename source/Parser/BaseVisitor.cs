namespace MOSESParser
{
	class BaseVisitor
	{
		enum AssignOp { EQ, PlusEq, SubEq, DivEq, ModEq, PowEq, ConcEq, OREq, ANDEq, XOREq, RSEq, LSEq }
		enum ExpressionType { STRING, NUMBER, FUNCTION, OTHER }
		class variableAssignClass { string variableName, valueName; AssignOp Op; ExpressionType valueType; }

		void variableAssign(variableAssignClass VAC) { }
	}
}