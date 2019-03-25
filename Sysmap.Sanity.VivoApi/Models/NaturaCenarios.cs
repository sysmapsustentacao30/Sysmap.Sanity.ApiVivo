using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sysmap.Sanity.VivoApi.Models
{
    public class NaturaCenarios
    {
        public int Numero_teste { get; set; }
        public string Sistema { get; set; }
        public string Ambiente { get; set; }
        public int Execucao_status { get; set; }
        public string Cn_login { get; set; }
        public string Cn_senha { get; set; }
        public string Gr_login { get; set; }
        public string Gr_senha { get; set; }
        public string Lider_login { get; set; }
        public string Lider_senha { get; set; }
        public string Browser { get; set; }

    }
}
