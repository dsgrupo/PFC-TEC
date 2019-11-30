using System;
using System.Text;

namespace PFC.SGP.Domain.Entities
{
    public class Trabalho : AbstractEntity
    {
        public Trabalho()
        {
            this.DataCadastro = DateTime.Now;
        }

        public string Nome { get; set; }

        public virtual Aluno Aluno { get; set; }

        public virtual Orientador Orientador { get; set; }

        public override string ToString()
        {
            //TODO
            StringBuilder msgCorpoEmail = new StringBuilder();
            return "Turma: " + Aluno.Turma.Codigo + msgCorpoEmail.Append("<p>") +
                "Curso: " + Aluno.Turma.Curso.Nome + msgCorpoEmail.Append("<p>") +
                "Aluno: " + Aluno.Nome + " " + Aluno.Sobrenome + msgCorpoEmail.Append("<p>") +
                "Orientador: " + Orientador.Nome + " " + Orientador.Sobrenome + msgCorpoEmail.Append("<br>");
        }
    }
}