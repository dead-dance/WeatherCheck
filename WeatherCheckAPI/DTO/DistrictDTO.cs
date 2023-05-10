using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeatherCheckAPI.DTO
{
    public class DistrictDTO
    {
        public int Id { get; set; }
        public int Division_id { get; set; }
        [MaxLength(60)]
        public string Name { get; set; }
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Bn_name { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }

    public class DistRoot
    {
        public List<DistrictDTO>? Districts { get; set; }
    }
}