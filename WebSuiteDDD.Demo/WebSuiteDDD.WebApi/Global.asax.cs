using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using WebSuiteDDD.Infrastructure.Common.ApplicationSettings;
using WebSuiteDDD.Infrastructure.Common.Emailing;
using WebSuiteDDD.SharedKernel.DomainEvents;
using WebSuiteDemo.Loadtesting.ApplicationServices.Implementations;
using WebSuiteDemo.Loadtesting.Domain.DomainEvents;

namespace WebSuiteDDD.WebApi
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			DomainEventMediator.RegisterDomainEventHandler(new TimetableChangedEmailEventHandler(new FakeEmailService()));
			DomainEventMediator.RegisterDomainEventHandler(new TimetableChangedRabbitMqMessagingEventHandler(new ConfigFileConfigurationRepository()));
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}
	}
}
