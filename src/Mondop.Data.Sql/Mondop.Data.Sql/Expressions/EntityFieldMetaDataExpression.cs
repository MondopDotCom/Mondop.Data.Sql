using Mondop.Data.Sql.MetaDataModel;
using Mondop.Expressions;
using Mondop.Guard;

namespace Mondop.Data.Sql.Expressions
{
    public class EntityFieldMetaDataExpression: Expression
    {
        public EntityFieldMetaDataExpression(EntityFieldMetaData entityFieldMetaData)
        {
            EntityFieldMetaData = Ensure.IsNotNull(entityFieldMetaData,nameof(entityFieldMetaData));
        }

        public EntityFieldMetaData EntityFieldMetaData { get; }
    }
}
