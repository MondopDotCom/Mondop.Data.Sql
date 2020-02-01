using Mondop.Data.Sql.Annotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mondop.Data.Sql.Tests
{
    public class TestPocoUndecorated
    {
        [Key]
        public Guid TestId { get; set; }
        public string DataField { get; set; }
        public decimal AmountField { get; set; }
        public byte[] BinaryData { get; set; }
        public byte[] ImageData { get; set; }

        [Ignore]
        public string IgnoreData { get; set; }

        [Timestamp]
        public byte[] RowVer { get; set; }
    }
}
