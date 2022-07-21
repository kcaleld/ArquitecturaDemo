using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquitecturaDemo.BLL.Extensions
{
    public class FilterInfo
    {
        public string Value { get; set; } = null!;
        public OperatorsEnum Operator { get; set; } = OperatorsEnum.Contains;
    }
}