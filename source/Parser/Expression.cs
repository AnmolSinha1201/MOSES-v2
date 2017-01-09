namespace MOSESParser
{
    partial class Parser
    {
        string Expression(string code, ref int origin)
        {
            return constantExpression(code, ref origin);
        }
        string constantExpression(string code, ref int origin)
        {
            return STRING(code, ref origin) ?? NUMBER(code, ref origin);
        }
    }
}