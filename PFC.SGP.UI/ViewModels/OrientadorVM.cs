using System;
using System.ComponentModel.DataAnnotations;

namespace PFC.SGP.UI.ViewModels
{
    public class OrientadorVM: AbstractEntity
    {
        public OrientadorVM()
        {
            this.DataCadastro = DateTime.Now;
        }

        [RegularExpression(@"^(([a-zA-Z0-9\u00C0-\u00FF]{1})+)$",
            ErrorMessage = "Este não parece um código válido, digite apenas o código, sem espaços.")]
        [StringLength(50)]
        [Required]
        [Display(Name = "Código")]
        public string Codigo { get; set; }

        [RegularExpression(@"^(([a-zA-Z\u00C0-\u00FF]{2,})+( ?[a-zA-Z\u00C0-\u00FF]+)+)$",
            ErrorMessage = "Este não parece um sobrenome válido.")]
        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [RegularExpression(@"^(([a-zA-Z\u00C0-\u00FF]{2,})+( ?[a-zA-Z\u00C0-\u00FF]+)+)$",
            ErrorMessage = "Este não parece um sobrenome válido.")]
        [Required]
        [StringLength(80)]
        public string Sobrenome { get; set; }

        [RegularExpression(@"(?:^\([0]?[1-9]{2}\)|^[0]?[1-9]{2}[\.-]?)[9]?[1-9]\d{3}[\.-]?\d{4}$",
            ErrorMessage = "Este não é um número válido.")]
        [Required]
        [StringLength(20)]
        public string Telefone { get; set; }

        [RegularExpression(@"\b[\w\.-]+@[\w\.-]+\.\w{2,4}\b",
            ErrorMessage = "Este não é um e-mail válido.")]
        [Required]
        [StringLength(50)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        

        public override string ToString()
        {
            return this.Codigo +" - " + this.Nome + " " + this.Sobrenome;
        }
    }
}
