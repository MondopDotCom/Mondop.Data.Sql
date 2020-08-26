using Mondop.Guard;

namespace Mondop.Data.Sql.MetaDataModel
{
    public class EntityFieldMetaData
    {
        public EntityFieldMetaData(EntityMetaData entity)
        {
            Entity = Ensure.IsNotNull(entity, nameof(entity));
        }

        public EntityMetaData Entity { get; }

        public string Name { get; set; }
        public string DbColumnName { get; set; }
        public string DbColumnType { get; set; }
        public string Size { get; set; }
        public string Precision { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsDatabaseGeneratedKey { get; set; }
        public bool IsRowVersion { get; set; }

        public string FormatQualifiedName(char sep)
        {
            var parentName = Entity.FormatQualifiedName(sep);

            return $"{parentName}{sep}{DbColumnName}";
        }

        public string FormatQualifiedName(char sep,char prefix,char suffix)
        {
            var parentName = Entity.FormatQualifiedName(sep, prefix, suffix);

            return $"{parentName}{sep}{prefix}{DbColumnName}{suffix}";
        }
    }
}
