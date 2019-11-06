using PFC.SGP.Domain.Contracts.Repositories;
using PFC.SGP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace PFC.SGP.Service
{
    public class MailService
    {
        private readonly ITrabalhoRepository _trabalhoRepository;

        public MailService(ITrabalhoRepository trabalhoRepository)
        {
            _trabalhoRepository = trabalhoRepository;
        }

        private List<Trabalho> ObterListaTrabalhos15Dias(DateTime dataAtual)
        {
            List<Trabalho> trabalhosAtivos = _trabalhoRepository.Find().ToList();
            List<Trabalho> trabalhos15Dias = new List<Trabalho>();

            DateTime dataMaxima;
            DateTime dataMinima;

            foreach (Trabalho trab in trabalhosAtivos)
            {
                dataMaxima = new DateTime(trab.Aluno.AnoApresentacao, trab.Aluno.MesApresentacao, 1);
                dataMinima = dataMaxima.AddDays(-15);
                if (dataAtual >= dataMinima && dataAtual < dataMaxima)
                {
                    trabalhos15Dias.Add(trab);
                }
            }

            return trabalhos15Dias;
        }

        public void EnviarNotificacaoPorEmail()
        {
            string mensagemFinalJarsia = "";
            string chamado = "Prezada Jarsia, o Sistema de Gerenciamento de Projetos(SGP) vem por meio deste email informar que os seguintes alunos entraram na área de risco (Zona Vermelha):";
            string infoTrabalhos = "";
            string despedida = "Atenciosamente, SGP!";

            List<Trabalho> trabalhos15Dias = ObterListaTrabalhos15Dias(DateTime.Now);

            string[] toStringTrabalho = new string[trabalhos15Dias.Count];
            StringBuilder msgCorpoEmail = new StringBuilder();

            for (int i = 0; i < trabalhos15Dias.Count; i++)
            {
                toStringTrabalho[i] = trabalhos15Dias[i].ToString();
            }


            for (int i = 0; i < trabalhos15Dias.Count; i++)
            {
                infoTrabalhos += toStringTrabalho[i] + msgCorpoEmail.Append("<br>");

            }

            mensagemFinalJarsia = chamado + msgCorpoEmail.Append("<b><br/>") + infoTrabalhos + msgCorpoEmail.Append("<b><br/>") + despedida;

            try
            {
                //Smpt Client Details
                SmtpClient clientDetails = new SmtpClient();
                clientDetails.Port = 587;
                clientDetails.Host = "smtp.live.com";
                clientDetails.EnableSsl = true;
                clientDetails.DeliveryMethod = SmtpDeliveryMethod.Network;
                clientDetails.UseDefaultCredentials = false;
                clientDetails.Credentials = new System.Net.NetworkCredential("piocarvalho_1@hotmail.com", "felipe12");

                //Message Details
                MailMessage mailDetails = new MailMessage();
                mailDetails.From = new MailAddress("piocarvalho_1@hotmail.com");
                mailDetails.To.Add("felipec798@gmail.com");

                mailDetails.Subject = "Alunos que entraram na zona vermelha (Zona de risco)";

                mailDetails.IsBodyHtml = true;
                mailDetails.Body = mensagemFinalJarsia;

                clientDetails.Send(mailDetails);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
