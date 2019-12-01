using PFC.SGP.Domain.Contracts.Repositories;
using PFC.SGP.Domain.Entities;
using PFC.SGP.UI.Validation;
using PFC.SGP.UI.ViewModels;
using PFC.SGP.UI.ViewModels.Home.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace PFC.SGP.UI.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Coordenador")]
    public class HomeController : Controller
    {

        private readonly ITrabalhoRepository _trabalhoRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly IOrientadorRepository _orientadorRepository;

        public HomeController(ITrabalhoRepository trabalhoRepository, IAlunoRepository alunoRepository, IOrientadorRepository orientadorRepository)
        {
            _trabalhoRepository = trabalhoRepository;
            _alunoRepository = alunoRepository;
            _orientadorRepository = orientadorRepository;
        }

        public ActionResult Index()
        {
            if (User.Identity.Name.Equals("admin"))
            {
                return RedirectToAction("Dashboard", "Home");
            }
            return RedirectToAction("Movimentacao", "Home");
        }

        public ActionResult Dashboard()
        {
            ViewBag.Trabalhos = ObterListaTrabalhos().ToTrabalhoDashboardVM();
            DateTime dataAtual = DateTime.Now;
            List<TrabalhoDashboardVM> trabalhosAtivos = ObterListaTrabalhos().ToTrabalhoDashboardVM().ToList();
            ViewBag.TrabalhosAtrasados = ObterListaTrabalhosAtrasados(dataAtual, trabalhosAtivos);
            ViewBag.Trabalhos15Dias = ObterListaTrabalhos15Dias(dataAtual, trabalhosAtivos);
            ViewBag.Trabalhos30Dias = ObterListaTrabalhos30Dias(dataAtual, trabalhosAtivos);
            ViewBag.Trabalhos90Dias = ObterListaTrabalhos90Dias(dataAtual, trabalhosAtivos);
            return View();
        }
        [CustomAuthorize(Roles = "Coordenador")]
        public ViewResult Movimentacao()
        {
            ViewBag.Trabalhos = ObterListaTrabalhos().ToTrabalhoDashboardVM();
            DateTime dataAtual = DateTime.Now;
            ViewBag.Trabalhos15DiasNotificacao = ObterListaTrabalhos15DiasNotificacao(dataAtual);
            ViewBag.Trabalhos30DiasNotificacao = ObterListaTrabalhos30DiasNotificacao(dataAtual);
            ViewBag.Trabalhos90DiasNotificacao = ObterListaTrabalhos90DiasNotificacao(dataAtual);
            return View();
        }


        public ViewResult Sobre()
        {
            return View();
        }

        private List<Trabalho> ObterListaTrabalhos()
        {
            return _trabalhoRepository.Find(User.Identity.Name)
                        .ToList();
        }

        private List<Aluno> ObterListaAlunos()
        {
            return _alunoRepository.Find(User.Identity.Name).ToList();
        }

        private List<Orientador> ObterListaOrientadores()
        {
            return _orientadorRepository.Find(User.Identity.Name).ToList();
        }

        private List<TrabalhoDashboardVM> ObterListaTrabalhosAtrasados(DateTime dataAtual, List<TrabalhoDashboardVM> trabalhosAtivos)
        {
            List<TrabalhoDashboardVM> trabalhosAtrasados = new List<TrabalhoDashboardVM>();

            DateTime dataMaxima;
            DateTime dataMinima;

            foreach (TrabalhoDashboardVM trab in trabalhosAtivos)
            {
                dataMaxima = new DateTime(int.Parse(trab.AnoApresentacao), int.Parse(trab.MesApresentacao), 1);
                dataMinima = DateTime.MinValue;
                if (dataAtual > dataMaxima)
                {
                    trabalhosAtrasados.Add(trab);
                }
            }

            return trabalhosAtrasados;
        }

        private List<TrabalhoDashboardVM> ObterListaTrabalhos15Dias(DateTime dataAtual, List<TrabalhoDashboardVM> trabalhosAtivos)
        {
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

            return trabalhos15Dias;
        }

        private List<TrabalhoDashboardVM> ObterListaTrabalhos30Dias(DateTime dataAtual, List<TrabalhoDashboardVM> trabalhosAtivos)
        {
            List<TrabalhoDashboardVM> trabalhos30Dias = new List<TrabalhoDashboardVM>();

            DateTime dataMaxima;
            DateTime dataMinima;

            foreach (TrabalhoDashboardVM trab in trabalhosAtivos)
            {
                dataMaxima = new DateTime(int.Parse(trab.AnoApresentacao), int.Parse(trab.MesApresentacao), 1).AddDays(-15);
                dataMinima = dataMaxima.AddDays(-15);
                if (dataAtual >= dataMinima && dataAtual < dataMaxima)
                {
                    trabalhos30Dias.Add(trab);
                }
            }

            return trabalhos30Dias;
        }

        private List<TrabalhoDashboardVM> ObterListaTrabalhos90Dias(DateTime dataAtual, List<TrabalhoDashboardVM> trabalhosAtivos)
        {
            List<TrabalhoDashboardVM> trabalhos30Dias = new List<TrabalhoDashboardVM>();

            DateTime dataMaxima;
            DateTime dataMinima;

            foreach (TrabalhoDashboardVM trab in trabalhosAtivos)
            {
                dataMaxima = new DateTime(int.Parse(trab.AnoApresentacao), int.Parse(trab.MesApresentacao), 1).AddDays(-30);
                dataMinima = dataMaxima.AddDays(-60);
                if (dataAtual >= dataMinima && dataAtual < dataMaxima)
                {
                    trabalhos30Dias.Add(trab);
                }
            }

            return trabalhos30Dias;
        }

        // Metodos Notificação

        private List<TrabalhoDashboardVM> ObterListaTrabalhos15DiasNotificacao(DateTime dataAtual)
        {
            List<TrabalhoDashboardVM> trabalhosAtivos = ObterListaTrabalhos().ToTrabalhoDashboardVM().ToList();
            List<TrabalhoDashboardVM> trabalhos15Dias = new List<TrabalhoDashboardVM>();

            DateTime dataMaxima;
            DateTime dataMinima;

            foreach (TrabalhoDashboardVM trab in trabalhosAtivos)
            {
                dataMaxima = new DateTime(int.Parse(trab.AnoApresentacao), int.Parse(trab.MesApresentacao), 1);
                TimeSpan diferenca = Convert.ToDateTime(dataMaxima) - Convert.ToDateTime(dataAtual);
                int distancia = diferenca.Days;
                if (distancia < 15 && distancia >= 5  )
                {
                    trabalhos15Dias.Add(trab);
                }
            }

            return trabalhos15Dias;
        }

        private List<TrabalhoDashboardVM> ObterListaTrabalhos30DiasNotificacao(DateTime dataAtual)
        {
            List<TrabalhoDashboardVM> trabalhosAtivos = ObterListaTrabalhos().ToTrabalhoDashboardVM().ToList();
            List<TrabalhoDashboardVM> trabalhos30Dias = new List<TrabalhoDashboardVM>();

            DateTime dataMaxima;
            DateTime dataMinima;

            foreach (TrabalhoDashboardVM trab in trabalhosAtivos)
            {
                dataMaxima = new DateTime(int.Parse(trab.AnoApresentacao), int.Parse(trab.MesApresentacao), 1);
                TimeSpan diferenca = Convert.ToDateTime(dataMaxima) - Convert.ToDateTime(dataAtual);
                int distancia = diferenca.Days;
                if (distancia < 30 && distancia >= 20)
                {
                    trabalhos30Dias.Add(trab);
                }
            }

            return trabalhos30Dias;
        }

        private List<TrabalhoDashboardVM> ObterListaTrabalhos90DiasNotificacao(DateTime dataAtual)
        {
            List<TrabalhoDashboardVM> trabalhosAtivos = ObterListaTrabalhos().ToTrabalhoDashboardVM().ToList();
            List<TrabalhoDashboardVM> trabalhos90Dias = new List<TrabalhoDashboardVM>();

            DateTime dataMaxima;
            DateTime dataMinima;

            foreach (TrabalhoDashboardVM trab in trabalhosAtivos)
            {
                dataMaxima = new DateTime(int.Parse(trab.AnoApresentacao), int.Parse(trab.MesApresentacao), 1);
                TimeSpan diferenca = Convert.ToDateTime(dataMaxima) - Convert.ToDateTime(dataAtual);
                int distancia = diferenca.Days;
                if (distancia < 90 && distancia >= 80)
                {
                    trabalhos90Dias.Add(trab);
                }
            }

            return trabalhos90Dias;
        }
        //public  void enviaremailparajarsia()
        //{
        //    string mensagemfinaljarsia = "";
        //    string chamado = "prezada jarsia, o sistema de gerenciamento de projetos(sgp) vem por meio deste email informar que os seguintes alunos entraram na área de risco (zona vermelha):";
        //    string despedida = "atenciosamente, sgp!";

        //    datetime dataatual = datetime.now;
        //    list<trabalhodashboardvm> trabalhosativos = obterlistatrabalhos().totrabalhodashboardvm().tolist();
        //    list<trabalhodashboardvm> trabalhos15dias = new list<trabalhodashboardvm>();

        //    datetime datamaxima;
        //    datetime dataminima;

        //    foreach (trabalhodashboardvm trab in trabalhosativos)
        //    {
        //        datamaxima = new datetime(int.parse(trab.anoapresentacao), int.parse(trab.mesapresentacao), 1);
        //        dataminima = datamaxima.adddays(-15);
        //        if (dataatual >= dataminima && dataatual < datamaxima)
        //        {
        //            trabalhos15dias.add(trab);
        //        }
        //    }

        //    string mensagem = "";
        //    string[] tostringtrabalho = new string[trabalhos15dias.count];
        //    stringbuilder msgcorpoemail = new stringbuilder();

        //    for (int i = 0; i < trabalhos15dias.count; i++)
        //    {
        //        tostringtrabalho[i] = trabalhos15dias[i].tostring();
        //    }


        //    for (int i = 0; i < trabalhos15dias.count; i++)
        //    {
        //        mensagem += tostringtrabalho[i] + msgcorpoemail.append("<br>");

        //    }

        //    mensagemfinaljarsia = chamado + msgcorpoemail.append("<b><br/>") + mensagem + msgcorpoemail.append("<b><br/>") + despedida;

        //    try
        //    {
        //        //smpt client details
        //        smtpclient clientdetails = new smtpclient();
        //        clientdetails.port = 587;
        //        clientdetails.host = "smtp.live.com";
        //        clientdetails.enablessl = true;
        //        clientdetails.deliverymethod = smtpdeliverymethod.network;
        //        clientdetails.usedefaultcredentials = false;
        //        clientdetails.credentials = new system.net.networkcredential("piocarvalho_1@hotmail.com", "felipe12");

        //        //message details
        //        mailmessage maildetails = new mailmessage();
        //        maildetails.from = new mailaddress("piocarvalho_1@hotmail.com");
        //        maildetails.to.add("felipec798@gmail.com");

        //        maildetails.subject = "alunos que entraram na zona vermelha (zona de risco)";

        //        maildetails.isbodyhtml = true;
        //        maildetails.body = mensagemfinaljarsia;

        //        clientdetails.send(maildetails);

        //    }
        //    catch (exception ex)
        //    {
        //        system.diagnostics.debug.writeline(ex.message);
        //    }
        //}
    }
}