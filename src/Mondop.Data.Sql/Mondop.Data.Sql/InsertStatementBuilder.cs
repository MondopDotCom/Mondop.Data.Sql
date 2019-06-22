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
                .Where(field => !field.IsDatabaseGeneratedKey)
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

            var databaseGeneratedKeys = metaData.PrimaryKey.Where(field => field.IsDatabaseGeneratedKey).ToArray();
            if (databaseGeneratedKeys.Length > 0)
            {
                sb.Append(" OUTPUT ");
                sb.Append(string.Join(",", databaseGeneratedKeys.Select(key => "inserted." + key.DbColumnName)));
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
