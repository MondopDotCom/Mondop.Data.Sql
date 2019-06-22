using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mondop.Data.Sql.Tests
{
    [Table("Table", Schema = "Schema" )]
    public class TestPoco
    {
        [Column("Id"),Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestId { get; set; }

        [Column("Data",TypeName ="varchar(200)")]
        public string DataField { get; set; }

        [Column("Amount",TypeName ="decimal(8,3")]
        public decimal AmountField { get; set; }

        [Column("Binary",TypeName ="nvarchar(max)")]
        public byte[] BinaryData { get; set; }

        [Column("Image",TypeName ="image")]
        public byte[] ImageData { get; set; }
    }
}
