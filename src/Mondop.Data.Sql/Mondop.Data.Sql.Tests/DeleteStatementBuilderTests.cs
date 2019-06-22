using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Mondop.Data.Sql.Tests
{
    [TestClass]
    public class DeleteStatementBuilderTests
    {
        private DeleteStatementBuilder deleteStatementBuilder;
        private readonly MetaDataModelBuilder _metaDataModelBuilder = new MetaDataModelBuilder(
            new AttributesMapping());


        [TestInitialize]
        public void TestInitialize()
        {
            deleteStatementBuilder = new DeleteStatementBuilder();
        }

        [TestMethod]
        public void CallBuild_WithPocoWithoutKeys_Expect_CorrectStatement()
        {
            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));
            testPocoMetaData.PrimaryKey.Clear();

            var result = deleteStatementBuilder.Build(testPocoMetaData);

            var sb = new StringBuilder();
            sb.AppendLine("DELETE FROM [Schema].[Table]");

            result.CommandText.Should().Be(sb.ToString());
            result.Parameters.Should().HaveCount(0);
        }

        [TestMethod]
        public void CallBuild_WithPocoWithKeys_Expect_CorrectStatement()
        {

            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));

            var result = deleteStatementBuilder.Build(testPocoMetaData);

            var sb = new StringBuilder();
            sb.AppendLine("DELETE FROM [Schema].[Table]");
            sb.AppendLine(" WHERE Id=@Id");

            result.CommandText.Should().Be(sb.ToString());
            result.Parameters.Should().HaveCount(1);
            result.Parameters.Should().ContainSingle(x => x.Name == "@Id");
        }
    }
}
