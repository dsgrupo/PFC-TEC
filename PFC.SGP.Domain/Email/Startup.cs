﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Owin;
using Owin;
using PFC.SGP.Domain.Contracts.Repositories;
using PFC.SGP.Domain.Entities;
using PFC.SGP.UI.ViewModels;
using PFC.SGP.UI.ViewModels.Home.Dashboard;
using System.Web.Mvc;
using PFC.SGP.UI.Controllers;

[assembly: OwinStartup(typeof(PFC.SGP.UI.Startup))]

namespace PFC.SGP.UI
{
    public class Startup
    {

        //private readonly ITrabalhoRepository _trabalhoRepository;
        //private readonly IUsuarioRepository _usuarioRepository;

        /*public Startup1(ITrabalhoRepository trabalhoRepository, IUsuarioRepository usuarioRepository)
        {
            this._trabalhoRepository = trabalhoRepository;
            this._usuarioRepository = usuarioRepository;
        }*/
        /*public List<Trabalho> ObterListaTrabalhos()
        {
            return _trabalhoRepository.Find()
                        .ToList();
        }*/

        public void EnviarEmailParaJarsia()
        {


            string mensagemFinalJarsia = "";
            string chamado = "Prezada Jarsia, o Sistema de Gerenciamento de Projetos(SGP) vem por meio deste email informar que os seguintes alunos entraram na área de risco (Zona Vermelha):";
            string despedida = "Atenciosamente, SGP!";

            DateTime dataAtual = DateTime.Now;
            List<TrabalhoDashboardVM> trabalhosAtivos = HomerController.ObterListaTrabalhos().ToTrabalhoDashboardVM().ToList();
            List<TrabalhoDashboardVM> trabalhos15Dias = new List<TrabalhoDashboardVM>();

            DateTime dataMaxima;
            DateTime dataMinima;

            foreach (TrabalhoDashboardVM trab in trabalhosAtivos)
            {
                dataMaxima = new DateTime(int.Parse(trab.AnoApresentacao), int.Parse(trab.MesApresentacao), 1);
                dataMinima = dataMaxima.AddDays(-15);
                if (dataAtual >= dataMinima && dataAtual < dataMaxima)
                {
                    trabalhos15Dias.Add(trab);
                }
            }

            string mensagem = "";
            string[] toStringTrabalho = new string[trabalhos15Dias.Count];
            StringBuilder msgCorpoEmail = new StringBuilder();

            for (int i = 0; i < trabalhos15Dias.Count; i++)
            {
                toStringTrabalho[i] = trabalhos15Dias[i].ToString();
            }


            for (int i = 0; i < trabalhos15Dias.Count; i++)
            {
                mensagem += toStringTrabalho[i] + msgCorpoEmail.Append("<br>");

            }

            mensagemFinalJarsia = chamado + msgCorpoEmail.Append("<b><br/>") + mensagem + msgCorpoEmail.Append("<b><br/>") + despedida;

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
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("devConn");

            //RecurringJob.AddOrUpdate(() => HomeController.EnviarEmailParaJarsia(), Cron.Minutely);
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
