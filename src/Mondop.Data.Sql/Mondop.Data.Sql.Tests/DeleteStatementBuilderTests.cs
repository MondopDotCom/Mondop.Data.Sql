using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Mondop.Data.Sql.Tests
{
    [TestClass]
    public class DeleteStatementBuilderTests
    {
        private DeleteStatementBuilder deleteStatementBuilder;
        private readonly MetaDataModelBuilder _metaDataModelBuilder = new MetaDataModelBuilder();


        [TestInitialize]
        public void TestInitialize()
        {
            deleteStatementBuilder = new DeleteStatementBuilder();
        }

        [TestMethod]
        public void CallBuild_WithPocoWithoutKeysAndRowVersion_Expect_CorrectStatement()
        {
            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));
            testPocoMetaData.PrimaryKey.Clear();
            deleteStatementBuilder.Options.UseRowVersionForDelete = false;

            var result = deleteStatementBuilder.Build(testPocoMetaData);

            var sb = new StringBuilder();
            sb.AppendLine("DELETE FROM [Schema].[Table]");

            result.CommandText.Should().Be(sb.ToString());
            result.Parameters.Should().HaveCount(0);
        }

        [TestMethod]
        public void CallBuild_WithPocoWithKeysWithoutRowVersion_Expect_CorrectStatement()
        {
            deleteStatementBuilder.Options.UseRowVersionForDelete = false;

            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));

            var result = deleteStatementBuilder.Build(testPocoMetaData);

            var sb = new StringBuilder();
            sb.AppendLine("DELETE FROM [Schema].[Table]");
            sb.AppendLine("WHERE Id=@Id");

            result.CommandText.Should().Be(sb.ToString());
            result.Parameters.Should().HaveCount(1);
            result.Parameters.Should().ContainSingle(x => x.Name == "@Id");
        }

        [TestMethod]
        public void CallBuild_WithPocoWithKeysWithRowVersion_Expect_CorrectStatement()
        {
            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));

            var result = deleteStatementBuilder.Build(testPocoMetaData);

            var sb = new StringBuilder();
            sb.AppendLine("DELETE FROM [Schema].[Table]");
            sb.AppendLine("WHERE Id=@Id AND RowVer=@RowVer");

            result.CommandText.Should().Be(sb.ToString());
            result.Parameters.Should().HaveCount(2);
            result.Parameters.Should().ContainSingle(x => x.Name == "@Id");
            result.Parameters.Should().ContainSingle(x => x.Name == "@RowVer");
        }
    }
}
