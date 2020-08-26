using Mondop.Expressions;

namespace Mondop.Data.Sql.Expressions
{
    public class ParameterExpression: Expression
    {
        public ParameterExpression(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
