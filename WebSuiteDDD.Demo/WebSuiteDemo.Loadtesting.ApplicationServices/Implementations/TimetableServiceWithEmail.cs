using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.Infrastructure.Common.Emailing;
using WebSuiteDemo.Loadtesting.ApplicationServices.Abstractions;
using WebSuiteDemo.Loadtesting.ApplicationServices.Messaging;
using WebSuiteDemo.Loadtesting.Domain;

namespace WebSuiteDemo.Loadtesting.ApplicationServices.Implementations
{
	public class TimetableServiceWithEmail : ITimetableService
	{
		private readonly ITimetableService _innerTimetableService;
		private readonly IEmailService _emailService;

		public TimetableServiceWithEmail(ITimetableService innerTimetableService, IEmailService emailService)
		{
			if (innerTimetableService == null) throw new ArgumentNullException("Inner timetable service");
			if (emailService == null) throw new ArgumentNullException("Email service");
			_innerTimetableService = innerTimetableService;
			_emailService = emailService;
		}
	
		public async Task<AddOrUpdateLoadtestsResponse> AddOrUpdateLoadtestsAsync(AddOrUpdateLoadtestsRequest addOrUpdateLoadtestsRequest)
		{
			AddOrUpdateLoadtestsResponse resp = await _innerTimetableService.AddOrUpdateLoadtestsAsync(addOrUpdateLoadtestsRequest);
			if (resp.Exception == null)
			{
				AddOrUpdateLoadtestsValidationResult addOrUpdateValidationResult = resp.AddOrUpdateLoadtestsValidationResult;
				if ((addOrUpdateValidationResult.ToBeInserted.Any() || addOrUpdateValidationResult.ToBeUpdated.Any())
					&& !addOrUpdateValidationResult.Failed.Any())
				{
					EmailArguments args = new EmailArguments("Load tests added or updated", "Load tests added or updated", "The Boss", "The developer", "123.456.678");
					_emailService.SendEmail(args);
				}
			}
			return resp;
		}

		public async Task<DeleteLoadtestResponse> DeleteLoadtestAsync(DeleteLoadtestRequest deleteLoadtestRequest)
		{
			return await _innerTimetableService.DeleteLoadtestAsync(deleteLoadtestRequest);
		}

		public async Task<GetLoadtestsForTimePeriodResponse> GetLoadtestsForTimePeriodAsync(GetLoadtestsForTimePeriodRequest getLoadtestsForTimePeriodRequest)
		{
			return await _innerTimetableService.GetLoadtestsForTimePeriodAsync(getLoadtestsForTimePeriodRequest);
		}
	}
}
