using Mondop.Expressions;
using System.IO;

namespace Mondop.Data.Sql
{
    public interface IExpressionToSqlWriter
    {
        void Write(StringWriter sw, Expression expression);
    }
}
