using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CDNMiddleware.DataAccess.Models
{
    [Table("users")]
    [Index(nameof(Username))]
    [Index(nameof(Mail))]
    [Index(nameof(PhoneNumber))]
    public class User : Base
	{
        [Column("username", TypeName = "VARCHAR(255)"), Required]
        public string? Username { get; set; }

        [Column("mail", TypeName = "VARCHAR(255)"), Required]
        public string? Mail { get; set; }

        [Column("phone_number", TypeName = "VARCHAR(255)"), Required]
        public string? PhoneNumber { get; set; }

        [Column("skillsets", TypeName = "VARCHAR(255)"), Required]
        public string? Skillsets { get; set; }

        [Column("hobby", TypeName = "VARCHAR(255)"), Required]
        public string? Hobby { get; set; }
    }
}

