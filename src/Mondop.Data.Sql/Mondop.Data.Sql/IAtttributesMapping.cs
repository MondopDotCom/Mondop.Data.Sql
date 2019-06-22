using System;

namespace Mondop.Data.Sql
{
    public interface IAttributeMapping
    {
        Type AttributeType { get; }
    }

    public interface ITableAttributeMapping: IAttributeMapping
    {
        string TableName(Attribute attribute);
        string SchemaName(Attribute attribute);
    }

    public interface IColumnAttributeMapping: IAttributeMapping
    {
        string ColumnName(Attribute attribute);
        string DataType(Attribute attribute);
    }

    public interface IAtttributesMapping
    {
        ITableAttributeMapping TableAttribute { get; }
        IColumnAttributeMapping ColumnAttribute { get; }
    }
}
