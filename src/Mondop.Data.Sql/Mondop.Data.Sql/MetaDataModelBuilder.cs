using Mondop.Data.Sql.MetaDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mondop.Guard;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mondop.Data.Sql
{
    public class MetaDataModelBuilder
    {
        private readonly IAtttributesMapping _atttributesMapping;

        private Dictionary<Type, Action<Attribute, EntityFieldMetaData>> _attributeHandlers = new Dictionary<Type, Action<Attribute, EntityFieldMetaData>>();
        public MetaDataModelBuilder(IAtttributesMapping atttributesMapping)
        {
            _atttributesMapping = Ensure.IsNotNull(atttributesMapping, nameof(atttributesMapping));

            _attributeHandlers.Add(typeof(KeyAttribute), (a, m) => { m.IsPrimaryKey = true; });
            _attributeHandlers.Add(typeof(DatabaseGeneratedAttribute), (a, m) => { m.IsDatabaseGeneratedKey = ((DatabaseGeneratedAttribute)a).DatabaseGeneratedOption == DatabaseGeneratedOption.Identity; });
            _attributeHandlers.Add(_atttributesMapping.ColumnAttribute.AttributeType, SetColumnAttribute);
        }

        public EntityMetaData Build(Type entityType)
        {
            var metaData = new EntityMetaData();

            GetTableName(entityType, metaData);
            GetFields(entityType, metaData);
            GetPrimaryKey(metaData);

            return metaData;
        }

        private void GetTableName(Type entityType, EntityMetaData metaData)
        {
            var typeInfo = entityType.GetTypeInfo();

            var tableAttribute = typeInfo.GetCustomAttribute(_atttributesMapping.TableAttribute.AttributeType);

            if (tableAttribute == null)
                throw new InvalidOperationException($"TableAttribute {_atttributesMapping.TableAttribute.AttributeType} not found on type {entityType}");

            metaData.SchemaName = _atttributesMapping.TableAttribute.SchemaName(tableAttribute);
            metaData.TableName = _atttributesMapping.TableAttribute.TableName(tableAttribute);
        }
        private void GetFields(Type entityType, EntityMetaData metaData)
        {
            var properties = entityType.GetProperties();

            foreach (var property in properties)
                GetField(property,metaData);
        }

        private void GetField(PropertyInfo propertyInfo,EntityMetaData metaData)
        {
            var fieldMetaData = new EntityFieldMetaData();
            fieldMetaData.Name = propertyInfo.Name;
            metaData.Fields.Add(fieldMetaData);

            var attributes = propertyInfo.GetCustomAttributes();
            foreach (var attribute in attributes)
            {
                var attributeType = attribute.GetType();
                if(_attributeHandlers.ContainsKey(attributeType))
                {
                    _attributeHandlers[attributeType](attribute, fieldMetaData);
                }
            }
        }
               
        private void GetPrimaryKey(EntityMetaData metaData)
        {
            metaData.PrimaryKey = metaData.Fields.Where(field => field.IsPrimaryKey).ToList();
        }

        private void SetColumnAttribute(Attribute a,EntityFieldMetaData m)
        {
            m.DbColumnName = _atttributesMapping.ColumnAttribute.ColumnName(a);
            SetColumnType(_atttributesMapping.ColumnAttribute.DataType(a), m);
        }

        private void SetColumnType(string dataType,EntityFieldMetaData m)
        {
            if (string.IsNullOrWhiteSpace(dataType))
                return;

            var items = dataType.Split(new[] { '(', ')', ',' },StringSplitOptions.RemoveEmptyEntries);

            m.DbColumnType = items[0];
            if (items.Length > 1)
                m.Size = items[1];
            if (items.Length > 2)
                m.Precision = items[2];
        }
    }
}
