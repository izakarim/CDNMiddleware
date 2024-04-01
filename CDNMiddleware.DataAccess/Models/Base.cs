using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDNMiddleware.DataAccess.Models
{
	public class Base
    {
        [Key, Column("id", Order = 0)]
        public Int64 ID { get; set; }

        [Column("created_at", TypeName = "Timestamp"), DataType(DataType.DateTime)]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at", TypeName = "Timestamp"), DataType(DataType.DateTime)]

        public DateTime? UpdatedAt { get; set; }

        [Column("deleted_at", TypeName = "Timestamp"), DataType(DataType.DateTime)]

        public DateTime? DeletedAt { get; set; }
    }
}

