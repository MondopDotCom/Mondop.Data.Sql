using Mondop.Data.Sql.MetaDataModel;
using System.Collections.Generic;
using System.Linq;

namespace Mondop.Data.Sql
{
    public class DmlWhereClauseBuilder
    {
        public List<EntityFieldMetaData> GetFields(EntityMetaData metaData,bool useRowVersion)
        {
            var whereFields = metaData.PrimaryKey;
            if (useRowVersion)
                whereFields = whereFields.Concat(metaData.Fields.Where(field => field.IsRowVersion)).Distinct().ToList();

            return whereFields;
        }

        public string Build(EntityMetaData metaData, bool useRowVersion)
        {
            var whereFields = GetFields(metaData,useRowVersion);

            if (whereFields.Count > 0)
                return "WHERE " + string.Join(" AND ", whereFields.Select(pk => pk.DbColumnName + "=@" + pk.DbColumnName));

            return string.Empty;
        }
    }
}
