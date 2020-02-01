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
            modelBuilder = new MetaDataModelBuilder();
        }

        [TestMethod]
        public void CallBuild_WithTestPoco_Expect_CorrectMetaData()
        {
            var result = modelBuilder.Build(typeof(TestPoco));
            result.SchemaName.Should().Be("Schema");
            result.TableName.Should().Be("Table");
            result.Fields.Should().HaveCount(6);

            result.Fields.Should().ContainSingle(x => x.Name == "TestId" && x.IsPrimaryKey && !x.IsRowVersion &&
            x.IsDatabaseGeneratedKey && x.DbColumnName == "Id" && x.DbColumnType == "int");

            result.Fields.Should().ContainSingle(x => (x.Name == "DataField") &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey && !x.IsRowVersion &&
            (x.DbColumnName == "Data") && (x.DbColumnType == "varchar") &&
            (x.Size == "200") && string.IsNullOrEmpty(x.Precision));

            result.Fields.Should().ContainSingle(x => (x.Name == "AmountField") &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey && !x.IsRowVersion &&
            (x.DbColumnName == "Amount") && (x.DbColumnType == "decimal") &&
            (x.Size == "8") && (x.Precision == "3"));

            result.Fields.Should().ContainSingle(x => (x.Name == "BinaryData") &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey && !x.IsRowVersion &&
            (x.DbColumnName == "Binary") && (x.DbColumnType == "varbinary") &&
            (x.Size == "max") && string.IsNullOrEmpty(x.Precision));

            result.Fields.Should().ContainSingle(x => (x.Name == "ImageData") &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey && !x.IsRowVersion &&
            (x.DbColumnName == "Image") && (x.DbColumnType == "image") &&
            x.Size =="max" && string.IsNullOrEmpty(x.Precision));

            result.Fields.Should().ContainSingle(x => x.Name == "RowVer" &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey && x.IsRowVersion &&
            x.DbColumnName == "RowVer" && x.DbColumnType == "Rowversion" &&
            x.Size == "max" && string.IsNullOrEmpty(x.Precision));

        }

        [TestMethod]
        public void CallBuild_WithTestPocoUndecorated_Expect_CorrectMetaData()
        {
            var result = modelBuilder.Build(typeof(TestPocoUndecorated));
            result.SchemaName.Should().BeEmpty();
            result.TableName.Should().Be("TestPocoUndecorated");
            result.Fields.Should().HaveCount(6);

            result.Fields.Should().ContainSingle(x => x.Name == "TestId" && x.IsPrimaryKey && !x.IsRowVersion &&
            !x.IsDatabaseGeneratedKey && x.DbColumnName == "TestId" && x.DbColumnType =="uniqueidentifier");

            result.Fields.Should().ContainSingle(x => (x.Name == "DataField") &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey && !x.IsRowVersion &&
            (x.DbColumnName == "DataField") && (x.DbColumnType == "nvarchar") &&
            (x.Size == "max") && string.IsNullOrEmpty(x.Precision));

            result.Fields.Should().ContainSingle(x => (x.Name == "AmountField") &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey && !x.IsRowVersion &&
            (x.DbColumnName == "AmountField") && (x.DbColumnType == "decimal") &&
            string.IsNullOrEmpty(x.Size) && string.IsNullOrEmpty(x.Precision));

            result.Fields.Should().ContainSingle(x => (x.Name == "BinaryData") &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey && !x.IsRowVersion &&
            (x.DbColumnName == "BinaryData") && (x.DbColumnType == "varbinary") &&
            (x.Size == "max") && string.IsNullOrEmpty(x.Precision));

            result.Fields.Should().ContainSingle(x => (x.Name == "ImageData") &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey && !x.IsRowVersion &&
            (x.DbColumnName == "ImageData") && (x.DbColumnType == "varbinary") &&
            x.Size == "max" && string.IsNullOrEmpty(x.Precision));

            result.Fields.Should().ContainSingle(x => x.Name == "RowVer" &&
            !x.IsPrimaryKey && !x.IsDatabaseGeneratedKey && x.IsRowVersion &&
            x.DbColumnName == "RowVer" && x.DbColumnType == "Rowversion" &&
            x.Size == "max" && string.IsNullOrEmpty(x.Precision));

        }
    }
}
