﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Mondop.Data.Sql.Tests
{
    [TestClass]
    public class UpdateStatementBuilderTests
    {
        private UpdateStatementBuilder _updateStatementBuilder;
        private readonly MetaDataModelBuilder _metaDataModelBuilder = new MetaDataModelBuilder();

        [TestInitialize]
        public void TestInitialize()
        {
            _updateStatementBuilder = new UpdateStatementBuilder();
        }

        [TestMethod]
        public void CallBuild_withoutKeys_Expect_CorrectCommand()
        {
            _updateStatementBuilder.Options.UseRowVersionForUpdate = false;

            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));
            testPocoMetaData.PrimaryKey.Clear();

            var expected = new StringBuilder();
            expected.AppendLine("UPDATE [Schema].[Table] SET");
            expected.AppendLine("Data=@Data, Amount=@Amount, Binary=@Binary, Image=@Image");

            var result = _updateStatementBuilder.Build(testPocoMetaData);
            result.CommandText.Should().Be(expected.ToString());
            result.Parameters.Should().HaveCount(4);
        }

        [TestMethod]
        public void CallBuild_withoutKeysWithRowVersion_Expect_CorrectCommand()
        {
            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));
            testPocoMetaData.PrimaryKey.Clear();

            var expected = new StringBuilder();
            expected.AppendLine("UPDATE [Schema].[Table] SET");
            expected.AppendLine("Data=@Data, Amount=@Amount, Binary=@Binary, Image=@Image");
            expected.AppendLine("OUTPUT inserted.RowVer");
            expected.AppendLine("WHERE RowVer=@RowVer");

            var result = _updateStatementBuilder.Build(testPocoMetaData);
            result.CommandText.Should().Be(expected.ToString());
            result.Parameters.Should().HaveCount(5);
        }

        [TestMethod]
        public void CallBuild_WithKeys_Expect_CorrectCommand()
        {
            _updateStatementBuilder.Options.UseRowVersionForUpdate = false;

            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));

            var expected = new StringBuilder();
            expected.AppendLine("UPDATE [Schema].[Table] SET");
            expected.AppendLine("Data=@Data, Amount=@Amount, Binary=@Binary, Image=@Image");
            expected.AppendLine("WHERE Id=@Id");

            var result = _updateStatementBuilder.Build(testPocoMetaData);
            result.CommandText.Should().Be(expected.ToString());
            result.Parameters.Should().HaveCount(5);
        }

        [TestMethod]
        public void CallBuild_WithKeysWithRowVersion_Expect_CorrectCommand()
        {
            var testPocoMetaData = _metaDataModelBuilder.Build(typeof(TestPoco));

            var expected = new StringBuilder();
            expected.AppendLine("UPDATE [Schema].[Table] SET");
            expected.AppendLine("Data=@Data, Amount=@Amount, Binary=@Binary, Image=@Image");
            expected.AppendLine("OUTPUT inserted.RowVer");
            expected.AppendLine("WHERE Id=@Id AND RowVer=@RowVer");

            var result = _updateStatementBuilder.Build(testPocoMetaData);
            result.CommandText.Should().Be(expected.ToString());
            result.Parameters.Should().HaveCount(6);
        }
    }
}
