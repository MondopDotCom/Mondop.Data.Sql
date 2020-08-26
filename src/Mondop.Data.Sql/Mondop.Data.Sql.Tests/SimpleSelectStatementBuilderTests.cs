using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;

namespace Mondop.Data.Sql.Tests
{
    [TestClass]
    public class SimpleSelectStatementBuilderTests
    {
        private SelectStatementBuilder builder;
        private readonly MetaDataModelBuilder _metaDataModelBuilder = new MetaDataModelBuilder();

        [TestInitialize]
        public void TestInitialize()
        {
            builder = new SelectStatementBuilder();
        }

        [TestMethod]
        public void SimpleSelect_Expect_CorrectQuery()
        {
            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));

            var command = builder.From(testPocoMetaData).Build();

            var expectedQuery = new StringBuilder();
            expectedQuery.AppendLine("SELECT *");
            expectedQuery.AppendLine("FROM [Schema].[Table]");

            command.CommandText.Should().Be(expectedQuery.ToString());
            command.Parameters.Should().HaveCount(0);
        }

        [TestMethod]
        public void SimpleSelectWithSingleParameter_Expect_CorrectQuery()
        {
            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));

            var command = builder.From(testPocoMetaData)
                .AddClause(eb => eb.AddEqualsTo(testPocoMetaData["RowVer"]))
                .Build();

            var expectedQuery = new StringBuilder();
            expectedQuery.AppendLine("SELECT *");
            expectedQuery.AppendLine("FROM [Schema].[Table]");
            expectedQuery.Append("WHERE [Schema].[Table].[RowVer]=@RowVer");

            command.CommandText.Should().Be(expectedQuery.ToString());
            command.Parameters.Should().HaveCount(1);
            command.Parameters[0].Name.Should().Be("@RowVer");
        }
    }
}
