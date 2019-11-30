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
        private readonly IUsuarioRepository _usuarioRepository;

        public MailService(ITrabalhoRepository trabalhoRepository, IUsuarioRepository usuarioRepository)
        {
            _trabalhoRepository = trabalhoRepository;
            _usuarioRepository = usuarioRepository;
        }

        private List<Usuario> ObterCoordenadores()
        {
            return this._usuarioRepository.Find().ToList();
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
            List<Trabalho> trabalhos15Dias = ObterListaTrabalhos15Dias(DateTime.Now);
            List<Usuario> coordenadores = ObterCoordenadores();


            if (trabalhos15Dias.Count > 0)
            {
                foreach (var coordenador in coordenadores)
                {
                    string chamado = "Prezado(a) " + coordenador.Nome + ", o Sistema de Gerenciamento de Projetos(SGP) vem por meio deste email informar que os seguintes alunos entraram na área de risco (Zona Vermelha):" + msgCorpoEmail.Append("<b>");
                    string corpoEmail = "";
                    string despedida = "Atenciosamente, SGP!";

                    List<Trabalho> trabalhosDasTurmasDoCoordenador = trabalhos15Dias.Where(t => t.Aluno.Turma.Curso.Coordenador.Id.Equals(coordenador.Id)).ToList();

                    foreach (var trabalhoCoordenado in trabalhosDasTurmasDoCoordenador)
                    {
                        corpoEmail += trabalhoCoordenado.ToString();
                    }

                    string mensagemFinal = chamado + msgCorpoEmail.Append("<p>") + corpoEmail + msgCorpoEmail.Append("<b>") + despedida;

                    try
                    {
                        //Smpt Client Details
                        SmtpClient clientDetails = new SmtpClient
                        {
                            Port = 587,
                            Host = "smtp.live.com",
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new System.Net.NetworkCredential("sistemadegerenciamentodeprojetos@hotmail.com", "sgp12345")
                        };

                        //Message Details
                        MailMessage mailDetails = new MailMessage
                        {
                            From = new MailAddress("sistemadegerenciamentodeprojetos@hotmail.com"),
                            Subject = "Alunos que entraram na zona vermelha (Zona de risco)",
                            IsBodyHtml = true,
                            Body = mensagemFinal

                        };
                        mailDetails.To.Add(coordenador.Email);

                        clientDetails.Send(mailDetails);

                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
            else
            {
                try
                {
                    foreach (var coordenador in coordenadores)
                    {

                        string chamado = "Prezado(a) " + coordenador.Nome + ", o Sistema de Gerenciamento de Projetos(SGP) vem por meio deste email informar que não existem alunos na área de risco(Zona Vermelha)" + msgCorpoEmail.Append("<b>");
                        string corpoEmail = "";
                        string despedida = "Atenciosamente, SGP!";
                        corpoEmail = chamado + msgCorpoEmail.Append("<b><br/>") + despedida;


                        //Smpt Client Details
                        SmtpClient clientDetails = new SmtpClient
                        {
                            Port = 587,
                            Host = "smtp.live.com",
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new System.Net.NetworkCredential("sistemadegerenciamentodeprojetos@hotmail.com", "sgp12345")
                        };

                        //Message Details
                        MailMessage mailDetails = new MailMessage
                        {
                            From = new MailAddress("sistemadegerenciamentodeprojetos@hotmail.com"),
                            Subject = "Alunos que entraram na zona vermelha (Zona de risco)",
                            IsBodyHtml = true,
                            Body = corpoEmail

                        };
                        mailDetails.To.Add(coordenador.Email);

                        clientDetails.Send(mailDetails);
                    }
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
