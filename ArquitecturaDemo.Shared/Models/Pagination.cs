using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaDemo.Shared.Models
{
    public class Pagination<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int OrderBy { get; set; }
        public bool OrderByDesc { get; set; }
        public List<T> Result { get; set; } = null!;
    }
}