﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Core.Entities
{
    [Table("ReusableProductId")]
    public class ReusableProductId
    {
        [Key]
        [Column(TypeName = "varchar(6)")]
        public string Id { get; set; } = null!;
    }
}