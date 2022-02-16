using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalMPA.Models.StoreViewModels
{
    public class PublisherIndexData
    {
        public IEnumerable<Publisher> Publishers { get; set; }
        public IEnumerable<Magazine> Magazines { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
