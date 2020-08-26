using Mondop.Expressions;
using Mondop.Guard;
using System.IO;

namespace Mondop.Data.Sql.Writers
{
    public class EqualToOperatorExpressionToSqlWriter : IExpressionToSqlWriter
    {
        private readonly ExpressionToSqlWritersFactory _factory;

        public EqualToOperatorExpressionToSqlWriter(ExpressionToSqlWritersFactory factory)
        {
            _factory = Ensure.IsNotNull(factory, nameof(factory));
        }

        public void Write(StringWriter sw, Expression expression)
        {
            var equalToOperationExpression = (EqualToOperatorExpression)expression;
            _factory.GetWriterFor(equalToOperationExpression.Left).Write(sw, equalToOperationExpression.Left);
            sw.Write("=");
            _factory.GetWriterFor(equalToOperationExpression.Right).Write(sw, equalToOperationExpression.Right);
        }
    }
}
