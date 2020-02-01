using Mondop.Data.Sql.MetaDataModel;
using Mondop.Data.Sql.Model;
using System.Linq;
using System.Text;

namespace Mondop.Data.Sql
{
    public class InsertStatementBuilder
    {
        public Command Build(EntityMetaData metaData)
        {
            var insertFields = metaData.Fields
                .Where(field => !field.IsDatabaseGeneratedKey && !field.IsRowVersion)
                .Select(field => field.DbColumnName)
                .ToArray();

            var sb = new StringBuilder();
            sb.Append("INSERT INTO [");
            sb.Append(metaData.SchemaName);
            sb.Append("].[");
            sb.Append(metaData.TableName);
            sb.Append("](");
            sb.Append(string.Join(",", insertFields));
            sb.Append(")");
            
            var outputFields = metaData.PrimaryKey.Where(field => field.IsDatabaseGeneratedKey).Concat(
                metaData.Fields.Where(field => field.IsRowVersion)).Select(field=> field.DbColumnName).Distinct().ToArray();
            if (outputFields.Length > 0)
            {
                sb.Append(" OUTPUT ");
                sb.Append(string.Join(",", outputFields.Select(field => "inserted." + field)));
            }

            sb.Append(" VALUES (");
            sb.Append(string.Join(",", insertFields.Select(field => "@" + field)));
            sb.Append(")");

            var result = new Command
            {
                CommandText = sb.ToString()
            };
            foreach (var field in insertFields)
                result.Parameters.Add(new CommandParameter { Name = "@" + field });

            return result;
        }
    }
}
