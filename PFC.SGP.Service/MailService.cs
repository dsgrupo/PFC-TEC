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
            StringBuilder msgCorpoEmail = new StringBuilder();
            string mensagemFinalJarsia = "";
            string chamado = "Prezada Jarsia, o Sistema de Gerenciamento de Projetos(SGP) vem por meio deste email informar que os seguintes alunos entraram na área de risco (Zona Vermelha):";
            string infoTrabalhos = "";
            string despedida = "Atenciosamente, SGP!";

            List<Trabalho> trabalhos15Dias = ObterListaTrabalhos15Dias(DateTime.Now);


            if (trabalhos15Dias.Count > 0)
            {
                string[] toStringTrabalho = new string[trabalhos15Dias.Count];

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
                    clientDetails.Credentials = new System.Net.NetworkCredential("sistemadegerenciamentodeprojetos@hotmail.com", "sgp12345");

                    //Message Details
                    MailMessage mailDetails = new MailMessage();
                    mailDetails.From = new MailAddress("sistemadegerenciamentodeprojetos@hotmail.com");
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
            else
            {
                try
                {
                    string mensagem_vazia = "";
                    string chamado_vazia = "Prezada Jarsia, o Sistema de Gerenciamento de Projetos(SGP) vem por meio deste email informar que não existem alunos na área de risco (Zona Vermelha)";
                    mensagem_vazia = chamado_vazia + msgCorpoEmail.Append("<b><br/>") + msgCorpoEmail.Append("<b><br/>") + despedida;

                    //Smpt Client Details
                    SmtpClient clientDetails = new SmtpClient();
                    clientDetails.Port = 587;
                    clientDetails.Host = "smtp.live.com";
                    clientDetails.EnableSsl = true;
                    clientDetails.DeliveryMethod = SmtpDeliveryMethod.Network;
                    clientDetails.UseDefaultCredentials = false;
                    clientDetails.Credentials = new System.Net.NetworkCredential("sistemadegerenciamentodeprojetos@hotmail.com", "sgp12345");

                    //Message Details
                    MailMessage mailDetails = new MailMessage();
                    mailDetails.From = new MailAddress("sistemadegerenciamentodeprojetos@hotmail.com");
                    mailDetails.To.Add("felipec798@gmail.com");

                    mailDetails.Subject = "Alunos que entraram na zona vermelha (Zona de risco)";

                    mailDetails.IsBodyHtml = true;
                    mailDetails.Body = mensagem_vazia;

                    clientDetails.Send(mailDetails);

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        public void EnviarEmailParaAlunos()
        {
            string mensagemFinalAluno = "";
            string chamado = "Prezado(a) aluno(a), o Sistema de Gerenciamento de Projetos(SGP) vem por meio deste email informar que a data da apresentação do seu trabalho está próxima, favor entrar em contato com o seu orientador";
            string despedida = "Atenciosamente, SGP!";

            List<Trabalho> trabalhos15Dias = ObterListaTrabalhos15Dias(DateTime.Now); ;

            DateTime dataMaxima;
            DateTime dataMinima;



            StringBuilder msgCorpoEmail = new StringBuilder();
            mensagemFinalAluno = chamado + msgCorpoEmail.Append("<b><br/>") + msgCorpoEmail.Append("<b><br/>") + despedida;

            try
            {

                SmtpClient clientDetails = new SmtpClient();
                clientDetails.Port = 587;
                clientDetails.Host = "smtp.live.com";
                clientDetails.EnableSsl = true;
                clientDetails.DeliveryMethod = SmtpDeliveryMethod.Network;
                clientDetails.UseDefaultCredentials = false;
                clientDetails.Credentials = new System.Net.NetworkCredential("sistemadegerenciamentodeprojetos@hotmail.com", "sgp12345");

                MailMessage mailDetails = new MailMessage();
                mailDetails.From = new MailAddress("sistemadegerenciamentodeprojetos@hotmail.com");

                foreach (Trabalho trab in trabalhos15Dias)
                {
                    mailDetails.To.Add(trab.Aluno.Email);
                    mailDetails.Subject = "Alerta de apresentação";

                    mailDetails.IsBodyHtml = true;
                    mailDetails.Body = mensagemFinalAluno;

                    clientDetails.Send(mailDetails);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
