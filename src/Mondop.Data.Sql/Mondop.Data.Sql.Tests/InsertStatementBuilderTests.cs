using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Mondop.Data.Sql.Tests
{
    [TestClass]
    public class InsertStatementBuilderTests
    {
        private InsertStatementBuilder _insertStatementBuilder;
        private readonly MetaDataModelBuilder _metaDataModelBuilder = new MetaDataModelBuilder();

        [TestInitialize]
        public void TestInitialize()
        {
            _insertStatementBuilder = new InsertStatementBuilder();
        }

        [TestMethod]
        public void CallBuild_WithoutDatabaseGeneratedKeys_ExpectCorrectQuery()
        {
            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));
            testPocoMetaData.PrimaryKey.Clear();

            var expected = new StringBuilder();
            expected.Append("INSERT INTO [Schema].[Table](Data,Amount,Binary,Image)");
            expected.Append(" OUTPUT inserted.RowVer");
            expected.Append(" VALUES (@Data,@Amount,@Binary,@Image)");

            var result = _insertStatementBuilder.Build(testPocoMetaData);
            result.CommandText.Should().Be(expected.ToString());
            result.Parameters.Should().HaveCount(4);
        }

        [TestMethod]
        public void CallBuild_WithDatabaseGeneratedKeys_ExpectCorrectQuery()
        {
            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));

            var expected = new StringBuilder();
            expected.Append("INSERT INTO [Schema].[Table](Data,Amount,Binary,Image)");
            expected.Append(" OUTPUT inserted.Id,inserted.RowVer");
            expected.Append(" VALUES (@Data,@Amount,@Binary,@Image)");

            var result = _insertStatementBuilder.Build(testPocoMetaData);
            result.CommandText.Should().Be(expected.ToString());
            result.Parameters.Should().HaveCount(4);
        }
    }
}
