using PFC.SGP.Domain.Contracts.Repositories;
using PFC.SGP.Domain.Entities;
using PFC.SGP.Domain.Security;
using PFC.SGP.UI.Validation;
using PFC.SGP.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PFC.SGP.UI.Controllers
{
    public class ContaController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICursoRepository _cursoRepository;

        public ContaController(IUsuarioRepository usuarioRepository, ICursoRepository cursoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _cursoRepository = cursoRepository;
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            List<UsuarioVM> contas = ObterListaUsuarios().ToUsuarioVM().ToList();
            ViewBag.Cursos = ObterListaCursos().ToCursoVM();
            return View(contas);
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Add()
        {
            ViewBag.Cursos = ObterListaCursos()
                                .ToCursoVM();
            return View(new UsuarioVM());
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Add(UsuarioVM usuarioRecebido, int[] idCursos)
        {
            bool loginExiste = ObterListaUsuarios().Where(u => u.Login.Equals(usuarioRecebido.Login, StringComparison.InvariantCultureIgnoreCase) && !u.Id.Equals(usuarioRecebido.Id)).ToList().Count > 0;
            bool codigoExiste = ObterListaUsuarios().Where(u => u.Codigo.Equals(usuarioRecebido.Codigo, StringComparison.InvariantCultureIgnoreCase) && !u.Id.Equals(usuarioRecebido.Id)).ToList().Count > 0;
            if (loginExiste)
            {
                ModelState.AddModelError("Login", "O login informado já existe.");
            }
            if (codigoExiste)
            {
                ModelState.AddModelError("Codigo", "Já existe um usuário com esse código.");
            }

            if (ModelState.IsValid)
            {
                Usuario usuarioSalvo = usuarioRecebido.ToUsuario();
                if (idCursos != null)
                {
                    foreach (int id in idCursos)
                    {
                        Curso curso = ObterListaCursos()
                                        .Where(c => c.Id.Equals(id))
                                        .FirstOrDefault();
                        if (curso == null) return HttpNotFound();
                        usuarioSalvo.Cursos.Add(curso);
                        _cursoRepository.Merge(curso);
                    }
                }
                else
                {
                    usuarioSalvo.Cursos = new List<Curso>();
                }
                usuarioSalvo.Senha = "12345@a".Encrypt();

                _usuarioRepository.Persist(usuarioSalvo);

                return RedirectToAction("Index");
            }
            ViewBag.Cursos = ObterListaCursos().ToCursoVM();

            return View(usuarioRecebido);
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(long id)
        {
            var usuario = _usuarioRepository.Find(id);
            if (usuario == null) return HttpNotFound();

            ViewBag.CursosNaoCoordenados = ObterListaCursos()
                                            .Where(c => c.Coordenador == null || !c.Coordenador.Id.Equals(id))
                                            .ToCursoVM()
                                            .ToList();
            ViewBag.CursosCoordenados = ObterListaCursos()
                                            .Where(c => c.Coordenador != null && c.Coordenador.Id.Equals(id))
                                            .ToCursoVM()
                                            .ToList();

            return View(usuario.ToUsuarioVM());
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(UsuarioVM usuarioRecebido, String resetar, int[] idCursos)
        {
            usuarioRecebido.Codigo = usuarioRecebido.Codigo.ToUpper();
            usuarioRecebido.Login = usuarioRecebido.Login.ToLower();
            Usuario usuarioSalvo = _usuarioRepository.Find(usuarioRecebido.Id);
            usuarioSalvo.Login = usuarioRecebido.Login;
            usuarioSalvo.Codigo = usuarioRecebido.Codigo;
            usuarioSalvo.Nome = usuarioRecebido.Nome;
            usuarioSalvo.Sobrenome = usuarioRecebido.Sobrenome;
            usuarioSalvo.Telefone = usuarioRecebido.Telefone;
            usuarioSalvo.Email = usuarioRecebido.Email;
            if (resetar != null) usuarioSalvo.Senha = "12345@a".Encrypt();

            bool loginExiste = ObterListaUsuarios().Where(u => u.Login.Equals(usuarioRecebido.Login, StringComparison.InvariantCultureIgnoreCase) && !u.Id.Equals(usuarioRecebido.Id)).ToList().Count > 0;
            bool codigoExiste = ObterListaUsuarios().Where(u => u.Codigo.Equals(usuarioRecebido.Codigo, StringComparison.InvariantCultureIgnoreCase) && !u.Id.Equals(usuarioRecebido.Id)).ToList().Count > 0;
            if (loginExiste)
            {
                ModelState.AddModelError("Login", "O login informado já existe.");
            }
            if (codigoExiste)
            {
                ModelState.AddModelError("Codigo", "Já existe um usuário com esse código.");
            }

            if (ModelState.IsValid)
            {
                if (idCursos != null)
                {
                    foreach (int id in idCursos)
                    {
                        Curso curso = ObterListaCursos().Where(c => c.Id.Equals(id)).FirstOrDefault();
                        if (curso == null) return HttpNotFound();
                        curso.Coordenador = usuarioSalvo;
                        _cursoRepository.Merge(curso);
                    }

                }
                else
                {
                    usuarioSalvo.Cursos = new List<Curso>();
                }

                _usuarioRepository.Merge(usuarioSalvo);

                return RedirectToAction("Index");
            }
            ViewBag.CursosNaoCoordenados = ObterListaCursos()
                .Where(c => c.Coordenador == null || !c.Coordenador.Id.Equals(usuarioRecebido.Id))
                .ToCursoVM().
                ToList();
            ViewBag.CursosCoordenados = ObterListaCursos()
                .Where(c => c.Coordenador != null && c.Coordenador.Id.Equals(usuarioRecebido.Id))
                .ToCursoVM()
                .ToList();

            return View(usuarioRecebido);
        }

        [HttpGet]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult DelCoord(long id)
        {
            var usuario = _usuarioRepository.Find(id);
            if (usuario == null) return HttpNotFound();
            if (usuario.Cursos.Count > 0)
            {
                return new HttpStatusCodeResult(400);
                //Verificação a ser feita em caso de exclusão lógica (atualmente não implementada)
                //foreach (var curso in usuario.Cursos)
                //{
                //    foreach (var turma in curso.Turmas)
                //    {
                //        if (turma.Ativo == 1)
                //        {
                //            return new HttpStatusCodeResult(400);
                //        }
                //    }

                //}
            }

            _usuarioRepository.Delete(usuario);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginVM usuarioLogando)
        {
            List<Usuario> usuarios = ObterTodosOsUsuarios();
            Usuario usuarioSalvo = usuarios.FirstOrDefault(u => u.Login == usuarioLogando.Login);

            if (usuarioSalvo == null)
                ModelState.AddModelError("Login", "Credenciais incorretas");
            else
            {
                if (usuarioSalvo.Senha != usuarioLogando.Senha.Encrypt())
                    ModelState.AddModelError("Login", "Credenciais incorretas");
            }

            if (ModelState.IsValid)
            {
                string ticket;
                if (usuarioSalvo.Login.Equals("admin"))
                    ticket = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(
                        1, usuarioLogando.Login, DateTime.Now, DateTime.Now.AddHours(2), usuarioLogando.PermanecerLogado, "Administrador"));
                else
                    ticket = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(
                        1, usuarioLogando.Login, DateTime.Now, DateTime.Now.AddHours(2), usuarioLogando.PermanecerLogado, "Coordenador"));
                HttpCookie cookie;
                if (usuarioLogando.PermanecerLogado)
                {
                    cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket) { Expires = DateTime.MaxValue };
                }
                else
                {
                    cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket);
                }
                Response.Cookies.Add(cookie);
                if (!String.IsNullOrEmpty(usuarioLogando.ReturnURL) && Url.IsLocalUrl(usuarioLogando.ReturnURL))
                {
                    return Redirect(usuarioLogando.ReturnURL);
                }
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador,Coordenador")]
        public ActionResult MudarSenha(string password, string confirmPassword)
        {
            password = password.Trim();
            confirmPassword = confirmPassword.Trim();
            if (password.Equals(confirmPassword) && password.Length > 0)
            {
                List<Usuario> usuarios = ObterTodosOsUsuarios();
                var usuario = usuarios.FirstOrDefault(u => u.Login == User.Identity.Name);
                usuario.Senha = confirmPassword.Encrypt();
                _usuarioRepository.Merge(usuario);
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            return Json("Senha fora do padrão!", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Sair()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        { }

        private List<Usuario> ObterListaUsuarios()
        {
            List<Usuario> usuarios = _usuarioRepository.Find().ToList();

            Usuario admin = _usuarioRepository.Find().Where(u => u.Login.Equals("admin")).First();
            usuarios.Remove(admin);

            return usuarios;
        }

        private List<Usuario> ObterTodosOsUsuarios()
        {
            List<Usuario> usuarios = _usuarioRepository.Find().ToList();

            return usuarios;
        }

        private List<Curso> ObterListaCursos()
        {
            List<Curso> cursos = _cursoRepository.Find().ToList();

            return cursos;
        }
    }
}
