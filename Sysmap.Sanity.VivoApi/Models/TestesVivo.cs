using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sysmap.Sanity.VivoApi.Models
{
    public class TestesVivo
    {
        [Key]
        public int IdTeste { get; set; }
        [Required]
        public int IdRelease { get; set; }
        [Required]
        public string CodRelease { get; set; }
        public int Excluido { get; set; }
        public int Cenario { get; set; }
        [Required]
        public int Prioridade { get; set; }
        [Required]
        public string Demanda { get; set; }
        [Required]
        public string Analista { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        [Display(Name = "Status")]
        public int Status { get; set; }
        [Required]
        public string Sistema { get; set; }
        [Required]
        public string Plataforma { get; set; }
        [Required]
        public string Funcionalidade { get; set; }
        [Required]
        public string Size { get; set; }
        [Required]
        [Display(Name = "Tipo Teste")]
        public string TipoTeste { get; set; }
        [Required]
        public string Descricao { get; set; }
        [Required]
        [Display(Name = "Resultado Esperado")]
        public string ResultEsperado { get; set; }
        [Display(Name = "Tipo Massa")]
        public string TipoMassa { get; set; }
        public string Massa { get; set; }
        public string Documento { get; set; }
        public string ICCID { get; set; }
        [Display(Name = "Linha Gerada")]
        public string LinhaGerada { get; set; }
        [Display(Name = "Solicitação Gerada")]
        public string SolicGerada { get; set; }
        public string Observacao { get; set; }
    }
}
