using Hangfire;
using Microsoft.Owin;
using Owin;
using PFC.SGP.Service;
using System;

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

            //"0 30 7 1/10 * ?"
            RecurringJob.AddOrUpdate<MailService>(x => x.EnviarNotificacaoPorEmail(), Cron.Minutely);
            //RecurringJob.AddOrUpdate<MailService>(x => x.EnviarEmailParaAlunos(), "0 35 13 1/10 * ?", TimeZoneInfo.FindSystemTimeZoneById("Bahia Standard Time"));
            //RecurringJob.AddOrUpdate<MailService>(x => x.EnviarNotificacaoPorEmail(), "0 30 13 1/10 * ?", TimeZoneInfo.FindSystemTimeZoneById("Bahia Standard Time"));
            //RecurringJob.AddOrUpdate<MailService>(x => x.EnviarEmailParaAlunos(), "0 35 13 1/10 * ?", TimeZoneInfo.FindSystemTimeZoneById("Bahia Standard Time"));

            app.UseHangfireDashboard();
            app.UseHangfireServer();

        }
    }
}