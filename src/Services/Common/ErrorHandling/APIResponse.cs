using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponseHandling
{
    public class ApiResponse<T> 
    {

        public T Response { get; set; }

        public string InfoMsg { get; set; }

        public string ErrMsg { get; set; }
        public List<string> SupportMessages { get; set; } = new List<string>();

    }
}
