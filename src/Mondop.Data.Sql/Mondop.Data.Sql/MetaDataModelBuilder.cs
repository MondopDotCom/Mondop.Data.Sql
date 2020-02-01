using Mondop.Data.Sql.MetaDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mondop.Data.Sql
{
    public class MetaDataModelBuilder
    {
        private Dictionary<Type, Action<Attribute, EntityFieldMetaData>> _attributeHandlers = new Dictionary<Type, Action<Attribute, EntityFieldMetaData>>();

        private Dictionary<Type, string> _defaultTypes = new Dictionary<Type, string>();
        public MetaDataModelBuilder()
        {
            _attributeHandlers.Add(typeof(KeyAttribute), (a, m) => { m.IsPrimaryKey = true; });
            _attributeHandlers.Add(typeof(DatabaseGeneratedAttribute), (a, m) => { m.IsDatabaseGeneratedKey = ((DatabaseGeneratedAttribute)a).DatabaseGeneratedOption == DatabaseGeneratedOption.Identity; });
            _attributeHandlers.Add(typeof(ColumnAttribute), (a,m) => { SetColumnAttribute((ColumnAttribute)a, m); });
            _attributeHandlers.Add(typeof(TimestampAttribute), (a,m) => { SetTimestampAttribute((TimestampAttribute)a, m); });

            _defaultTypes.Add(typeof(int), "int");
            _defaultTypes.Add(typeof(Guid), "uniqueidentifier");
            _defaultTypes.Add(typeof(decimal), "decimal");
            _defaultTypes.Add(typeof(byte[]), "varbinary(max)");
            _defaultTypes.Add(typeof(string), "nvarchar(max)");
            _defaultTypes.Add(typeof(DateTime), "datetime");
            _defaultTypes.Add(typeof(bool), "bit");
            _defaultTypes.Add(typeof(short), "smallint");
            _defaultTypes.Add(typeof(byte), "tinyint");

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

            var tableAttribute = typeInfo.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;

            if (tableAttribute == null)
            {
                metaData.SchemaName = "";
                metaData.TableName = entityType.Name;
                return;
            }

            metaData.SchemaName = tableAttribute.Schema;
            metaData.TableName = tableAttribute.Name;
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
            fieldMetaData.DbColumnName = propertyInfo.Name;
            GetDefaultSqlType(propertyInfo.PropertyType,fieldMetaData);
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

        private void GetDefaultSqlType(Type propertyType, EntityFieldMetaData m)
        {
            string defaultType = "";
            if (_defaultTypes.ContainsKey(propertyType))
                defaultType = _defaultTypes[propertyType];

            SetColumnType(defaultType, m);
        }
               
        private void GetPrimaryKey(EntityMetaData metaData)
        {
            metaData.PrimaryKey = metaData.Fields.Where(field => field.IsPrimaryKey).ToList();
        }

        private void SetColumnAttribute(ColumnAttribute a,EntityFieldMetaData m)
        {
            m.DbColumnName = a.Name;
            SetColumnType(a.TypeName, m);
        }

        private void SetTimestampAttribute(TimestampAttribute a, EntityFieldMetaData m)
        {
            m.IsRowVersion = true;
            SetColumnType("Rowversion",m);
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
