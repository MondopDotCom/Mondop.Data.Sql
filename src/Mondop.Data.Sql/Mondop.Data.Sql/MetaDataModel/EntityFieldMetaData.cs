namespace Mondop.Data.Sql.MetaDataModel
{
    public class EntityFieldMetaData
    {
        public string Name { get; set; }
        public string DbColumnName { get; set; }
        public string DbColumnType { get; set; }
        public string Size { get; set; }
        public string Precision { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsDatabaseGeneratedKey { get; set; }
        public bool IsRowVersion { get; set; }
    }
}
