using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalMPA.Models
{
    public class PublishedMagazine
    {
        public int PublisherID { get; set; }
        public int MagazineID { get; set; }
        public Publisher Publisher { get; set; }
        public Magazine Magazine { get; set; }
    }
}
