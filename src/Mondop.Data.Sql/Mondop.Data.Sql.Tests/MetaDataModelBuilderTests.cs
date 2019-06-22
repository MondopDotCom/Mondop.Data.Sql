using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mondop.Data.Sql.Tests
{
    [TestClass]
    public class MetaDataModelBuilderTests
    {
        private MetaDataModelBuilder modelBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            modelBuilder = new MetaDataModelBuilder(new AttributesMapping());
        }

        [TestMethod]
        public void CallConstructor_WithInvalidArguments_ExpectArgumentNullException()
        {
            Action action1 = () => new MetaDataModelBuilder(null);

            action1.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void CallBuild_WithTestPoco_Expect_CorrectMetaData()
        {
            var result = modelBuilder.Build(typeof(TestPoco));
            result.SchemaName.Should().Be("Schema");
            result.TableName.Should().Be("Table");
            result.Fields.Should().HaveCount(5);

            result.Fields.Should().ContainSingle(x => x.Name == "TestId" && x.IsPrimaryKey &&
            x.IsDatabaseGeneratedKey && x.DbColumnName == "Id" && string.IsNullOrEmpty(x.DbColumnType));

            result.Fields.Should().ContainSingle(x => (x.Name == "DataField") &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey &&
            (x.DbColumnName == "Data") && (x.DbColumnType == "varchar") &&
            (x.Size == "200") && string.IsNullOrEmpty(x.Precision));

            result.Fields.Should().ContainSingle(x => (x.Name == "AmountField") &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey &&
            (x.DbColumnName == "Amount") && (x.DbColumnType == "decimal") &&
            (x.Size == "8") && (x.Precision == "3"));

            result.Fields.Should().ContainSingle(x => (x.Name == "BinaryData") &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey &&
            (x.DbColumnName == "Binary") && (x.DbColumnType == "nvarchar") &&
            (x.Size == "max") && string.IsNullOrEmpty(x.Precision));

            result.Fields.Should().ContainSingle(x => (x.Name == "ImageData") &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey &&
            (x.DbColumnName == "Image") && (x.DbColumnType == "image") &&
            string.IsNullOrEmpty(x.Size) && string.IsNullOrEmpty(x.Precision));

        }
    }
}
