using System;
using System.Collections.Generic;
using System.Linq;

namespace Mondop.Data.Sql.MetaDataModel
{
    public class EntityMetaData
    {
        public Type EntityType { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public List<EntityFieldMetaData> Fields { get; set; } = new List<EntityFieldMetaData>();
        public List<EntityFieldMetaData> PrimaryKey { get; set; } = new List<EntityFieldMetaData>();
        public EntityFieldMetaData this[string name] => Fields.SingleOrDefault(x=> x.Name.Equals(name));

        public string FormatQualifiedName(char sep)
        {
            if (string.IsNullOrWhiteSpace(SchemaName))
                return TableName;

            return $"{SchemaName}{sep}{TableName}";
        }

        public string FormatQualifiedName(char sep,char prefix,char suffix)
        {
            if (string.IsNullOrWhiteSpace(SchemaName))
                return $"{prefix}{TableName}{suffix}";

            return $"{prefix}{SchemaName}{suffix}{sep}{prefix}{TableName}{suffix}";
        }
    }
}
