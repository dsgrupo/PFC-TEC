using PFC.SGP.Domain.Contracts.Repositories;
using PFC.SGP.Domain.Entities;
using PFC.SGP.UI.Validation;
using PFC.SGP.UI.ViewModels;
using PFC.SGP.UI.ViewModels.Home.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ViewResult Index()
        {
            ViewBag.Trabalhos = ObterListaTrabalhos().ToTrabalhoDashboardVM();
            DateTime dataAtual = DateTime.Now;
            ViewBag.Trabalhos15Dias = ObterListaTrabalhos15Dias(dataAtual);
            ViewBag.Trabalhos30Dias = ObterListaTrabalhos30Dias(dataAtual);
            ViewBag.Trabalhos90Dias = ObterListaTrabalhos90Dias(dataAtual);
            return View();
        }

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

        private List<TrabalhoDashboardVM> ObterListaTrabalhos15Dias(DateTime dataAtual)
        {
            List<TrabalhoDashboardVM> trabalhosAtivos = ObterListaTrabalhos().ToTrabalhoDashboardVM().ToList();
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

        private List<TrabalhoDashboardVM> ObterListaTrabalhos30Dias(DateTime dataAtual)
        {
            List<TrabalhoDashboardVM> trabalhosAtivos = ObterListaTrabalhos().ToTrabalhoDashboardVM().ToList();
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

        private List<TrabalhoDashboardVM> ObterListaTrabalhos90Dias(DateTime dataAtual)
        {
            List<TrabalhoDashboardVM> trabalhosAtivos = ObterListaTrabalhos().ToTrabalhoDashboardVM().ToList();
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
                if (distancia <=15 && distancia > 5  )
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
                if (distancia <= 30 && distancia > 20)
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
                if (distancia <= 90 && distancia > 80)
                {
                    trabalhos90Dias.Add(trab);
                }
            }

            return trabalhos90Dias;
        }
    }
}