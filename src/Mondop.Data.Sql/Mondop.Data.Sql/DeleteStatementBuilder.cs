using Mondop.Core;
using Mondop.Data.Sql.MetaDataModel;
using Mondop.Data.Sql.Model;
using Mondop.Guard;
using System.Text;

namespace Mondop.Data.Sql
{
    public class DeleteStatementBuilder
    {
        private readonly WhereClauseBuilder _whereClauseBuilder = new WhereClauseBuilder();

        public DeleteStatementBuilder()
        {

        }

        public DeleteStatementBuilder(StatementBuilderOptions options)
        {
            Options = Ensure.IsNotNull(options, nameof(options));
        }

        public Command Build(EntityMetaData metaData)
        {
            var sb = new StringBuilder();
            sb.Append("DELETE FROM [");
            sb.Append(metaData.SchemaName);
            sb.Append("].[");
            sb.Append(metaData.TableName);
            sb.AppendLine("]");

            sb.AppendLineIfNotNullOrWhiteSpace(_whereClauseBuilder.Build(metaData, Options.UseRowVersionForDelete));

            var result = new Command
            {
                CommandText = sb.ToString()
            };

            var parameterFields = _whereClauseBuilder.GetFields(metaData, Options.UseRowVersionForDelete);
            foreach (var pkField in parameterFields)
                result.Parameters.Add(new CommandParameter { Name = "@" + pkField.DbColumnName });

            return result;
        }
        public StatementBuilderOptions Options { get; } = new StatementBuilderOptions();
    }
}
