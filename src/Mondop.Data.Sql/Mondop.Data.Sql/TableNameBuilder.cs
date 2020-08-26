using Mondop.Data.Sql.MetaDataModel;

namespace Mondop.Data.Sql
{
    public class TableNameBuilder
    {
        public string Build(EntityMetaData metaData)
        {
            if (!string.IsNullOrWhiteSpace(metaData.SchemaName))
                return $"[{metaData.SchemaName}].[{metaData.TableName}]";

            return $"[{metaData.TableName}]";
        }
    }
}
