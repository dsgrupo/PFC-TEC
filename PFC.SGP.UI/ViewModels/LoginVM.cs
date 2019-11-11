using System.ComponentModel.DataAnnotations;

namespace PFC.SGP.UI.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "O {0} é obrigatório")]
        [RegularExpression(@"^(([a-zA-Z0-9\u00C0-\u00FF.]{1})+)$",
            ErrorMessage = "Este não parece um login válido, digite apenas o login com caracteres alfanuméricos e pontos, sem espaços.")]
        [StringLength(100)]
        public string Login { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório")]
        [StringLength(80)]
        public string Senha { get; set; }

        public bool PermanecerLogado { get; set; }

        public string ReturnURL { get; set; }
    }
}
