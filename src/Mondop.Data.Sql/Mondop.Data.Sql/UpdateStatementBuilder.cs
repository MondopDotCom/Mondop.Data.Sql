using Mondop.Core;
using Mondop.Data.Sql.MetaDataModel;
using Mondop.Data.Sql.Model;
using Mondop.Guard;
using System.Linq;
using System.Text;

namespace Mondop.Data.Sql
{
    public class UpdateStatementBuilder
    {
        private readonly DmlWhereClauseBuilder _whereClauseBuilder = new DmlWhereClauseBuilder();
        private readonly TableNameBuilder _tableNameBuilder = new TableNameBuilder();

        public UpdateStatementBuilder()
        {

        }

        public UpdateStatementBuilder(StatementBuilderOptions options)
        {
            Options = Ensure.IsNotNull(options, nameof(options));
        }

        public Command Build(EntityMetaData metaData)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"UPDATE {_tableNameBuilder.Build(metaData)} SET");

            var updatedFields = metaData.Fields.
                Where(field => !field.IsPrimaryKey && !field.IsRowVersion);

            sb.AppendLine(string.Join(", ",updatedFields.Select(field =>
             field.DbColumnName + "=@" + field.DbColumnName)));

            if (Options.UseRowVersionForUpdate)
            {
                var outputFields = metaData.Fields.Where(field => field.IsRowVersion).Select(field => field.DbColumnName).Distinct().ToArray();
                if (outputFields.Length > 0)
                {
                    sb.Append("OUTPUT ");
                    sb.Append(string.Join(",", outputFields.Select(field => "inserted." + field)));
                    sb.AppendLine();
                }
            }

            sb.AppendLineIfNotNullOrWhiteSpace(_whereClauseBuilder.Build(metaData, Options.UseRowVersionForUpdate));

            var result = new Command
            {
                CommandText = sb.ToString()
            };

            var parameterFields = updatedFields.Concat( _whereClauseBuilder.GetFields(metaData, Options.UseRowVersionForUpdate));
            foreach (var pkField in parameterFields)
                result.Parameters.Add(new CommandParameter { Name = "@" + pkField.DbColumnName });

            return result;
        }

        public StatementBuilderOptions Options { get; } = new StatementBuilderOptions();
    }
}
