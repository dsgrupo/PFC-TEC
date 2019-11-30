using PFC.SGP.Domain.Entities;
using PFC.SGP.UI.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PFC.SGP.UI.ViewModels
{
    public class CursoVM : AbstractEntity
    {
        public CursoVM()
        {
            this.DataCadastro = DateTime.Now;
        }

        [Required, StringLength(40)]
        [RegularExpression(@"^(([a-zA-Z0-9\u00C0-\u00FF]{1,})+( ?[a-zA-Z0-9\u00C0-\u00FF]+)+)$",
            ErrorMessage = "Este não parece um nome válido.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = " O campo quantidade de semestres é obrigatório.")]
        [MinValue(1, ErrorMessage = "O valor minimo de semestres é 1.")]
        [Display(Name = "Quantidade de Semestres")]
        public int QtdSemestres { get; set; }


        public virtual ICollection<Turma> Turmas { get; set; } = new List<Turma>();

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
