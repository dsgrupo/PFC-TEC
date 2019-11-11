using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PFC.SGP.UI.ViewModels
{
    public class UsuarioVM : AbstractEntity
    {
        public UsuarioVM()
        {
            this.DataCadastro = DateTime.Now;
        }

        [Required, StringLength(100)]
        [RegularExpression(@"^(([a-zA-Z0-9\u00C0-\u00FF.]{1})+)$",
            ErrorMessage = "Este não parece um login válido, digite apenas o login com caracteres alfanuméricos e pontos, sem espaços.")]
        public string Login { get; set; }

        [Required, StringLength(50)]
        [RegularExpression(@"^(([a-zA-Z0-9\u00C0-\u00FF]{1})+)$",
            ErrorMessage = "Este não parece um código válido, digite apenas o código, sem espaços.")]
        public string Codigo { get; set; }

        [RegularExpression(@"^(([a-zA-Z\u00C0-\u00FF]{2,})+( ?[a-zA-Z\u00C0-\u00FF]+)+)$",
            ErrorMessage = "Este não parece um nome válido.")]
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
        public string Email { get; set; }

        public ICollection<Domain.Entities.Curso> Cursos { get; set; } = new List<Domain.Entities.Curso>();

        public override string ToString()
        {
            return this.Nome + " " + this.Sobrenome;
        }
        public string ObterCursos()
        {
            string cursos = "";
            foreach(Domain.Entities.Curso curso in this.Cursos)
            {
                cursos += curso.ToString() + ", ";
            }
            if(cursos.Length > 0) cursos = cursos.Remove(cursos.Length - 2, 2);
            return cursos;
        }
    }
}
