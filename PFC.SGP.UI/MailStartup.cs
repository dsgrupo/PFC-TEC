using Hangfire;
using Microsoft.Owin;
using Owin;
using PFC.SGP.Service;

[assembly: OwinStartup(typeof(PFC.SGP.UI.MailStartup))]

namespace PFC.SGP.UI
{
    public class MailStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = UnityConfig.GetConfiguredContainer();

            GlobalConfiguration.Configuration.UseSqlServerStorage("devConn");
            GlobalConfiguration.Configuration.UseUnityActivator(container);

            // Para um tempo maior, podemos utilizar Cron.DayInterval(10)
            RecurringJob.AddOrUpdate<MailService>(x => x.EnviarNotificacaoPorEmail(), Cron.DayInterval(10));
            RecurringJob.AddOrUpdate<MailService>(x => x.EnviarEmailParaAlunos(), Cron.DayInterval(10));
            app.UseHangfireDashboard();
            app.UseHangfireServer();

        }
    }
}