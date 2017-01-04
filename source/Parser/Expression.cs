namespace MOSESParser
{
    partial class Parser
    {
        object Expression(string code, ref int origin)
        {
            return constantExpression(code, ref origin);
        }
        object constantExpression(string code, ref int origin)
        {
            return STRING(code, ref origin) ?? NUMBER(code, ref origin);
        }
    }
}