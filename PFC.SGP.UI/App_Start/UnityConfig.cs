using Hangfire;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.SqlServer;
using Hangfire.States;
using PFC.SGP.Data.EF;
using PFC.SGP.Data.EF.Repositories;
using PFC.SGP.Domain.Contracts.Repositories;
using System;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace PFC.SGP.UI
{
    public static class UnityConfig
    {

        private static Lazy<IUnityContainer> container;

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static void RegisterComponents()
        {
            container = new Lazy<IUnityContainer>(() =>
            {
                var container = new UnityContainer();
                RegisterTypes(container);
                return container;
            });
        }
        public static void RegisterTypes(IUnityContainer container)
        {

            container.RegisterType<IUsuarioRepository, UsuarioRepositoryEF>();
            container.RegisterType<ICursoRepository, CursoRepositoryEF>();
            container.RegisterType<IAlunoRepository, AlunoRepositoryEF>();
            container.RegisterType<ITurmaRepository, TurmaRepositoryEF>();
            container.RegisterType<ITrabalhoRepository, TrabalhoRepositoryEF>();
            container.RegisterType<IOrientadorRepository, OrientadorRepositoryEF>();
            RegisterHangfire(container);
            PFCSGPDataContext context = new PFCSGPDataContext();
            container.RegisterInstance<PFCSGPDataContext>(context);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static void RegisterHangfire(IUnityContainer container)
        {
            container.RegisterType<IBackgroundJobClient, BackgroundJobClient>();
            container.RegisterType<JobStorage, SqlServerStorage>(new InjectionConstructor("devConn"));
            container.RegisterType<IJobFilterProvider, JobFilterAttributeFilterProvider>(new InjectionConstructor(true));
            container.RegisterType<IBackgroundJobFactory, BackgroundJobFactory>();
            container.RegisterType<IRecurringJobManager, RecurringJobManager>();
            container.RegisterType<IBackgroundJobStateChanger, BackgroundJobStateChanger>();
        }

    }
}