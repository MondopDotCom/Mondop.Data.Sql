using Mondop.Data.Sql.MetaDataModel;
using Mondop.Data.Sql.Model;
using System.Linq;
using System.Text;

namespace Mondop.Data.Sql
{
    public class UpdateStatementBuilder
    {
        public Command Build(EntityMetaData metaData)
        {
            var sb = new StringBuilder();
            sb.Append("UPDATE [");
            sb.Append(metaData.SchemaName);
            sb.Append("].[");
            sb.Append(metaData.TableName);
            sb.AppendLine("] SET");

            sb.AppendLine(string.Join(", ", metaData.Fields.
                Where(field=> !field.IsPrimaryKey).Select(field =>
            field.DbColumnName + "=@" + field.DbColumnName)));

            if(metaData.PrimaryKey.Count>0)
            {
                sb.AppendLine("WHERE " +  string.Join(" AND ",
                    metaData.PrimaryKey.Select(pk => pk.DbColumnName + "=@" + pk.DbColumnName)));
            }

            var result = new Command
            {
                CommandText = sb.ToString()
            };
            foreach (var field in metaData.Fields)
                result.Parameters.Add(new CommandParameter { Name = "@" + field.DbColumnName });

            return result;
        }
    }
}
