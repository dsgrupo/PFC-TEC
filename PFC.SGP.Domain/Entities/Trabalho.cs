using System;

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
            return base.ToString();
        }
    }
}