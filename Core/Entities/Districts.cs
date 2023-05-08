﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Core.Entities
{
    public class Districts : BaseEntity
    {
        public int DivisionId { get; set; }
        [MaxLength(60)]
        public string Name { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string BnName { get; set; }
        public Int64 Latitude { get; set; }
        public Int64 Longitude { get; set; }

    }
}