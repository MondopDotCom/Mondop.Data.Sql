using Mondop.Data.Sql.MetaDataModel;
using Mondop.Data.Sql.Model;
using System.Linq;
using System.Text;

namespace Mondop.Data.Sql
{
    public class DeleteStatementBuilder
    {
        public Command Build(EntityMetaData metaData)
        {
            var sb = new StringBuilder();
            sb.Append("DELETE FROM [");
            sb.Append(metaData.SchemaName);
            sb.Append("].[");
            sb.Append(metaData.TableName);
            sb.AppendLine("]");

            if (metaData.PrimaryKey.Count > 0)
            {
                sb.AppendLine(" WHERE " + string.Join(" AND ",
                    metaData.PrimaryKey.Select(pk => pk.DbColumnName + "=@" + pk.DbColumnName)));
            }

            var result = new Command
            {
                CommandText = sb.ToString()
            };
            foreach (var pkField in metaData.PrimaryKey)
                result.Parameters.Add(new CommandParameter { Name = "@" + pkField.DbColumnName });

            return result;
        }
    }
}
