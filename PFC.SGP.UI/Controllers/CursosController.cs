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
    [CustomAuthorize(Roles = "Administrador")]
    public class CursosController : Controller
    {
        private readonly ICursoRepository _cursoRepository;

        public CursosController(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<CursoVM> cursos = ObterListaCursos()
                                    .ToCursoVM()
                                    .ToList();
            return View(cursos);
        }

        [HttpPost]
        public ActionResult Add(CursoVM cursoRecebido)
        {
            bool nomeExistente = ObterListaCursos()
                .FirstOrDefault(c => c.Nome.Equals(cursoRecebido.Nome, StringComparison.InvariantCultureIgnoreCase) && !c.Id.Equals(cursoRecebido.Id)) != null;
            if (nomeExistente)
            {
                ModelState.AddModelError("Nome", "Já existe um curso com esse nome.");
            }
            if (ModelState.IsValid)
            {
                Curso novoCurso = cursoRecebido.ToCurso();
                _cursoRepository.Persist(novoCurso);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            List<Object> errorList = new List<Object>();
            foreach(var values in ModelState.Values)
            {
                foreach(var error in values.Errors)
                {
                    errorList.Add(new { error = error.ErrorMessage });
                }
            }
            return Json(errorList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(CursoVM cursoRecebido)
        {
            Curso cursoSalvo = _cursoRepository.Find(cursoRecebido.Id);
            if (cursoSalvo == null) return HttpNotFound();

            bool nomeExistente = ObterListaCursos()
                .FirstOrDefault(c => c.Nome.Equals(cursoRecebido.Nome, StringComparison.InvariantCultureIgnoreCase) && !c.Id.Equals(cursoRecebido.Id)) != null;
            if (nomeExistente)
            {
                ModelState.AddModelError("Nome", "Já existe um curso com esse nome");
            }

            if (ModelState.IsValid)
            {
                Curso novoCurso = cursoRecebido.ToCurso();
                cursoSalvo.Nome = novoCurso.Nome;
                cursoSalvo.QtdSemestres = novoCurso.QtdSemestres;
                _cursoRepository.Merge(cursoSalvo);
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            return Json("false", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DelCurso(long id)
        {
            var curso = _cursoRepository.Find(id);

            if (curso == null) return HttpNotFound();
            if (curso.Turmas.Count > 0)
            {
                return new HttpStatusCodeResult(400);
                //Verificação a ser feita em caso de exclusão lógica (atualmente não implementada)
                //if (curso.Coordenador.Ativo == 1)
                //{
                //    return new HttpStatusCodeResult(400);
                //}
            }

            _cursoRepository.Delete(curso);

            return null;
        }

        protected override void Dispose(bool disposing)
        {

        }

        private List<Curso> ObterListaCursos()
        {
            List<Curso> cursos = _cursoRepository.Find().ToList();
            return cursos;
        }
    }
}
