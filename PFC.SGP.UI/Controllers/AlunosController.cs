using PFC.SGP.Domain.Contracts.Repositories;
using PFC.SGP.Domain.Entities;
using PFC.SGP.UI.Validation;
using PFC.SGP.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PFC.SGP.UI.Controllers
{
    [CustomAuthorize(Roles = "Coordenador")]
    public class AlunosController : Controller
    {
        private readonly IAlunoRepository _alunoRepository;
        private readonly ITurmaRepository _turmaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public AlunosController(IAlunoRepository alunoRepository, ITurmaRepository turmaRepository, IUsuarioRepository usuarioRepository)
        {
            _alunoRepository = alunoRepository;
            _turmaRepository = turmaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public ViewResult Index()
        {
            var alunos = ObterListaAlunos()
                            .ToAlunoVM()
                            .ToList();
            return View(alunos);
        }

        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.turmas = ObterListaTurmas()
                                .ToList();
            return View(new AlunoVM() { AnoIngresso = 2000, MesIngresso = 1 });
        }

        [HttpPost]
        public ActionResult Add(AlunoVM alunoRecebido, string codTurma)
        {
            var query = ObterListaTurmas().Where(t => t.Codigo.Equals(codTurma)).ToList();
            if (query.Count == 0) return HttpNotFound();

            Turma turmaRecebida = query.First();
            Aluno novoAluno = alunoRecebido.ToAluno(turmaRecebida);
            ModelState["Turma"].Errors.Clear();

            bool matriculaExistente = ObterListaAlunos()
                .FirstOrDefault(a => a.Matricula.Equals(novoAluno.Matricula, StringComparison.InvariantCultureIgnoreCase) && !a.Id.Equals(novoAluno.Id)) != null;

            if (matriculaExistente)
            {
                ModelState.AddModelError("Matricula", "Já existe um aluno com essa matrícula");
            }

            ModelState["AnoApresentacao"].Errors.Clear();
            novoAluno.AnoApresentacao = ObterPrevisaoInicialApresentacao(novoAluno)[0];

            ModelState["MesApresentacao"].Errors.Clear();
            novoAluno.MesApresentacao = ObterPrevisaoInicialApresentacao(novoAluno)[1];

            if (ModelState.IsValid)
            {
                _alunoRepository.Persist(novoAluno);
                return RedirectToAction("Index");
            }
            ViewBag.turmas = ObterListaTurmas()
                                .ToTurmaVM();
            return View(novoAluno.ToAlunoVM());
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var usuarioAutenticado = ObterListaUsuarios().Where(u => u.Login.Equals(User.Identity.Name)).FirstOrDefault();
            var aluno = ObterListaAlunos().Where(a => a.Id == id).FirstOrDefault();

            if (aluno == null) return RedirectToAction("E404", "Erro");
            if (aluno.Turma.Curso.Coordenador.Id != usuarioAutenticado.Id) return RedirectToAction("Unauthorized", "Erro");

            ViewBag.turmas = ObterListaTurmas()
                                .ToTurmaVM();
            ViewBag.DataAntiga = new int[] { aluno.AnoApresentacao, aluno.MesApresentacao };

            return View(aluno.ToAlunoVM());
        }

        [HttpPost]
        public ActionResult Edit(AlunoVM alunoRecebido, string codTurma)
        {
            var query = ObterListaTurmas().Where(t => t.Codigo.Equals(codTurma)).ToList();
            if (query.Count == 0) return HttpNotFound();

            Turma turmaRecebida = query.First();
            Aluno novoAluno = alunoRecebido.ToAluno(turmaRecebida);
            ModelState["Turma"].Errors.Clear();

            bool matriculaExistente = ObterListaAlunos()
                .FirstOrDefault(a => a.Matricula.Equals(novoAluno.Matricula, StringComparison.InvariantCultureIgnoreCase) && !a.Id.Equals(novoAluno.Id)) != null;

            if (matriculaExistente)
            {
                ModelState.AddModelError("Matricula", "Já existe um aluno com essa matrícula");
            }
            novoAluno.AnoApresentacao = alunoRecebido.AnoApresentacao;
            ModelState["AnoApresentacao"].Errors.Clear();

            novoAluno.MesApresentacao = alunoRecebido.MesApresentacao;
            ModelState["MesApresentacao"].Errors.Clear();

            Aluno alunoSalvo = ObterListaAlunos()
                                        .Where(a => a.Id == novoAluno.Id)
                                        .FirstOrDefault();

            if (ObterPrevisaoInicialApresentacao(alunoSalvo)[0] > alunoRecebido.AnoApresentacao)
            {
                ModelState.AddModelError("MesApresentacao", "Restauramos a combinação Ano/Semestre pois a inserida não representava uma data posterior a data de previsão padrão");
            }
            else if (ObterPrevisaoInicialApresentacao(alunoSalvo)[0] == alunoRecebido.AnoApresentacao && ObterPrevisaoInicialApresentacao(alunoSalvo)[1] > alunoRecebido.MesApresentacao)
            {
                ModelState.AddModelError("MesApresentacao", "Restauramos a combinação Ano/Semestre pois a inserida não representava uma data posterior a data de previsão padrão");
            }

            if (ModelState.IsValid)
            {
                bool podeEditar = ObterListaAlunos().Where(a => a.Id == alunoSalvo.Id).FirstOrDefault() != null;
                if (!podeEditar) return RedirectToAction("Unauthorized", "Erro");

                _alunoRepository.Update(alunoSalvo, novoAluno, query.First());
                return RedirectToAction("Index");
            }
            ViewBag.DataAntiga = new int[] { alunoSalvo.AnoApresentacao, alunoSalvo.MesApresentacao };
            ViewBag.turmas = ObterListaTurmas()
                                .ToTurmaVM();

            novoAluno.AnoApresentacao = alunoSalvo.AnoApresentacao;
            novoAluno.MesApresentacao = novoAluno.MesApresentacao;

            return View(novoAluno.ToAlunoVM());
        }

        public ActionResult DelAluno(long id)
        {
            var usuarioAutenticado = ObterListaUsuarios().Where(u => u.Login.Equals(User.Identity.Name)).FirstOrDefault();
            Aluno alunoSalvo = ObterListaAlunos().Where(a => a.Id == id).FirstOrDefault();

            bool podeEditar = ObterListaAlunos().Where(a => a.Id == alunoSalvo.Id).FirstOrDefault() != null;
            if (!podeEditar) return RedirectToAction("Unauthorized", "Erro");
            if (alunoSalvo.Turma.Curso.Coordenador.Id != usuarioAutenticado.Id) return RedirectToAction("Unauthorized", "Erro");

            Turma turma = alunoSalvo.Turma;
            if (alunoSalvo.Trabalhos.Count > 0)
            {
                return new HttpStatusCodeResult(400);
                //Verificação a ser feita em caso de exclusão lógica (atualmente não implementada)
                //foreach (var trab in alunoSalvo.Trabalhos)
                //{
                //    if (trab.Ativo == 1)
                //    {
                //        return new HttpStatusCodeResult(400);
                //    }
                //}
            }

            _alunoRepository.Delete(alunoSalvo);

            return null;
        }

        private List<Aluno> ObterListaAlunos()
        {
            return _alunoRepository.Find(User.Identity.Name).ToList();
        }

        private List<Turma> ObterListaTurmas()
        {
            return _turmaRepository.Find(User.Identity.Name).ToList();
        }

        private List<Usuario> ObterListaUsuarios()
        {
            return _usuarioRepository.Find().ToList();
        }

        protected override void Dispose(bool disposing)
        { }

        private static int[] ObterPrevisaoInicialApresentacao(Aluno aluno)
        {
            const int TEMPO_ADICIONAL_PARA_APRESENTACAO = 3;//Em meses
            int ano = aluno.AnoIngresso;
            int mes = aluno.MesIngresso;
            int mesesCorridosFaltando = (aluno.Turma.Curso.QtdSemestres*6)+TEMPO_ADICIONAL_PARA_APRESENTACAO;
            while (mesesCorridosFaltando > 0)
            {
                if (mes == 12)
                {
                    ano++;
                    mes = 1;
                }
                else
                {
                    mes++;
                }
                mesesCorridosFaltando--;
            }

            return new int[] { ano, mes };
        }
    }
}