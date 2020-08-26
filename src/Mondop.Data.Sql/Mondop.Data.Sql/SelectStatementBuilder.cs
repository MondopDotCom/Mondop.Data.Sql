using Mondop.Data.Sql.Expressions;
using Mondop.Data.Sql.MetaDataModel;
using Mondop.Data.Sql.Model;
using Mondop.Data.Sql.Writers;
using Mondop.Expressions;
using Mondop.Guard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mondop.Data.Sql
{
    public class ExpressionBuilder
    {
        public ExpressionBuilder AddEqualsTo(EntityFieldMetaData field)
        {
            Expressions.Add(new EqualToOperatorExpression
            {
                Left = new EntityFieldMetaDataExpression(field),
                Right = new ParameterExpression("@" + field.Name)
            });
            return this;
        }

        public List<Expression> Expressions { get; } = new List<Expression>();
    }

    public class ExpressionToSqlWritersFactory
    {
        private readonly Dictionary<Type, IExpressionToSqlWriter> _expressionWriters = new Dictionary<Type, IExpressionToSqlWriter>();

        public ExpressionToSqlWritersFactory()
        {
            _expressionWriters.Add(typeof(EqualToOperatorExpression), new EqualToOperatorExpressionToSqlWriter(this));
            _expressionWriters.Add(typeof(EntityFieldMetaDataExpression), new EntityFieldMetaDataExpressionToSqlWriter(this));
            _expressionWriters.Add(typeof(ParameterExpression), new ParameterExpressionToSqlWriter(this));
        }

        public IExpressionToSqlWriter GetWriterFor(Expression expression)
        {
            Ensure.IsNotNull(expression, nameof(expression));

            if (!_expressionWriters.ContainsKey(expression.GetType()))
                throw new NotSupportedException($"No writer for expression type: {expression.GetType()}");

            return _expressionWriters[expression.GetType()];
        }
    }

    public class SelectStatementBuilder
    {
        private EntityMetaData _from;
        private List<EntityFieldMetaData> _select = new List<EntityFieldMetaData>();
        private readonly TableNameBuilder _tableNameBuilder = new TableNameBuilder();
        private readonly List<Expression> _expressions = new List<Expression>();

        public SelectStatementBuilder()
        {

        }

        public SelectStatementBuilder From(EntityMetaData from)
        {
            _from = from;
            return this;
        }

        public SelectStatementBuilder AddClause(Action<ExpressionBuilder> action)
        {
            var expressionBuilder = new ExpressionBuilder();
            action(expressionBuilder);
            _expressions.AddRange(expressionBuilder.Expressions);
            return this;
        }

        public Command Build()
        {
            var stringBuilder = new StringBuilder();
            var sw = new StringWriter(stringBuilder);
            BuildSelect(sw);
            BuildFrom(sw);
            BuildWhere(sw);

            var result = new Command
            {
                CommandText = stringBuilder.ToString(),
                Parameters = GetCommandParameters()
            };
            return result;
        }

        private void BuildSelect(StringWriter sw)
        {
            sw.Write("SELECT ");
            if (_select.Count == 0)
            {
                sw.WriteLine("*");
                return;
            }

            sw.WriteLine(string.Join(",", _select.Select(f => $"[{f.DbColumnName}]")));
        }

        private void BuildFrom(StringWriter sw)
        {
            sw.WriteLine($"FROM {_tableNameBuilder.Build(_from)}");
        }

        private void BuildWhere(StringWriter sw)
        {
            if (_expressions.Count == 0)
                return;

            var factory = new ExpressionToSqlWritersFactory();

            sw.Write("WHERE ");
            foreach (var expression in _expressions)
            {
                var writer = factory.GetWriterFor(expression);
                writer.Write(sw, expression);
            }
        }

        private List<CommandParameter> GetCommandParameters()
        {
            var parameters = _expressions
                .SelectMany(x => x.Find(typeof(ParameterExpression)))
                .Cast<ParameterExpression>()
                .Select(x => x.Name).Distinct()
                .Select(x =>  new CommandParameter { Name = x }).ToList();

            return parameters;
        }
    }
}
