using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeWheel.Filters
{
    public class MovieSearchFilter
    {
        public string title { get; set; }
        public int year { get; set; }
        public IEnumerable<Guid> genres { get; set; }
    }
}
