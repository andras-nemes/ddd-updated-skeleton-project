using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDemo.Loadtesting.Domain.DomainEvents;
using WebSuiteDDD.SharedKernel.DomainEvents;
using WebSuiteDDD.Infrastructure.Common.Emailing;
using WebSuiteDemo.Loadtesting.Domain;

namespace WebSuiteDemo.Loadtesting.ApplicationServices.Implementations
{
	public class TimetableChangedEmailEventHandler : IDomainEventHandler
	{
		private readonly IEmailService _emailService;

		public TimetableChangedEmailEventHandler(IEmailService emailService)
		{
			if (emailService == null) throw new ArgumentNullException("Email service");
			_emailService = emailService;
		}

		public void Handle(EventArgs eventArgs)
		{
			TimetableChangedEventArgs e = eventArgs as TimetableChangedEventArgs;
			if (e != null)
			{
				AddOrUpdateLoadtestsValidationResult addOrUpdateValidationResult = e.AddOrUpdateLoadtestsValidationResult;
				if ((addOrUpdateValidationResult.ToBeInserted.Any() || addOrUpdateValidationResult.ToBeUpdated.Any())
						&& !addOrUpdateValidationResult.Failed.Any())
				{
					EmailArguments args = new EmailArguments("Load tests added or updated", "Load tests added or updated", "The Boss", "The developer", "123.456.678");
					_emailService.SendEmail(args);
				}
			}
		}
	}
}
