using System;
using System.Collections.Generic;
using System.Text;

namespace Mondop.Data.Sql.MetaDataModel
{
    public class EntityMetaData
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public List<EntityFieldMetaData> Fields { get; set; } = new List<EntityFieldMetaData>();
        public List<EntityFieldMetaData> PrimaryKey { get; set; } = new List<EntityFieldMetaData>();
    }
}
