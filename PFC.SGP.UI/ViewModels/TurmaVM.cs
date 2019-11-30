using System;
using System.ComponentModel.DataAnnotations;

namespace PFC.SGP.UI.ViewModels
{
    public class TurmaVM : AbstractEntity
    {
        public TurmaVM()
        {
            this.DataCadastro = DateTime.Now;
        }

        [RegularExpression(@"^(([a-zA-Z0-9\u00C0-\u00FF]{1})+)$",
            ErrorMessage = "Este não parece um código válido, digite apenas o código, sem espaços.")]
        [StringLength(50)]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [Required]
        public string Curso { get; set; }

        public long AlunosCadastrados { get; set; }

        public override string ToString()
        {
            return this.Codigo;
        }

        
    }
}
