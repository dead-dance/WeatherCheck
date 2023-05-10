using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Core.Entities
{
    public class Districts : BaseEntity
    {
        public int Division_id { get; set; }
        [MaxLength(60)]
        public string Name { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Bn_name { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }

    }
}