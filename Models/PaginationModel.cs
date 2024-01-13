using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PaginationModel<T>
    {
        public int Page { get; set; }

        public int Total { get; set; }  

        public int PageCount { get; set; }

        public List<T>? Data { get; set; }
    }
}
