using Mondop.Data.Sql.Expressions;
using Mondop.Expressions;
using Mondop.Guard;
using System.IO;

namespace Mondop.Data.Sql.Writers
{
    public class EntityFieldMetaDataExpressionToSqlWriter : IExpressionToSqlWriter
    {
        private readonly ExpressionToSqlWritersFactory _factory;

        public EntityFieldMetaDataExpressionToSqlWriter(ExpressionToSqlWritersFactory factory)
        {
            _factory = Ensure.IsNotNull(factory, nameof(factory));
        }

        public void Write(StringWriter sw, Expression expression)
        {
            sw.Write(((EntityFieldMetaDataExpression)expression)
                .EntityFieldMetaData
                .FormatQualifiedName('.', '[', ']'));
        }
    }
}
