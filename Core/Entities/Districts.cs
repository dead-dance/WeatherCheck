using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Districts : BaseEntity
    {
        public int DivisionId { get; set; }
        public string Name { get; set; }
        public string BnName { get; set; }
        public Int64 Latitude { get; set; }
        public Int64 Longitude { get; set; }

    }
}