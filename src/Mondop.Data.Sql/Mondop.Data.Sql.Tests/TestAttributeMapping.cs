using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mondop.Data.Sql.Tests
{
    public class TableAttributeMapping : ITableAttributeMapping
    {
        public Type AttributeType => typeof(TableAttribute);

        public string SchemaName(Attribute attribute)
        {
            if (attribute is TableAttribute tableAttribute)
                return tableAttribute.Schema;

            throw new InvalidCastException("Attribute is not a TableAttribute");
        }

        public string TableName(Attribute attribute)
        {
            if (attribute is TableAttribute tableAttribute)
                return tableAttribute.Name;

            throw new InvalidCastException("Attribute is not a TableAttribute");
        }
    }

    public class ColumnAttributeMapping : IColumnAttributeMapping
    {
        public Type AttributeType => typeof(ColumnAttribute);

        public string ColumnName(Attribute attribute)
        {
            if (attribute is ColumnAttribute columnAttribute)
                return columnAttribute.Name;

            throw new InvalidCastException("Attribute is not a ColumnAttribute");
        }

        public string DataType(Attribute attribute)
        {
            if (attribute is ColumnAttribute columnAttribute)
                return columnAttribute.TypeName;

            throw new InvalidCastException("Attribute is not a ColumnAttribute");
        }
    }

    public class AttributesMapping : IAtttributesMapping
    {
        public ITableAttributeMapping TableAttribute { get; } = new TableAttributeMapping();
        public IColumnAttributeMapping ColumnAttribute { get; } = new ColumnAttributeMapping();
    }
}
