using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebSuiteDDD.WebApi.Models;
using WebSuiteDemo.Loadtesting.ApplicationServices.Abstractions;
using WebSuiteDemo.Loadtesting.ApplicationServices.Messaging;
using WebSuiteDemo.Loadtesting.Domain;

namespace WebSuiteDDD.WebApi.Controllers
{
    public class LoadtestsController : ApiController
    {
		private readonly ITimetableService _timetableService;

		public LoadtestsController(ITimetableService timetableService)
		{
			if (timetableService == null) throw new ArgumentNullException("ITimetableService");
			_timetableService = timetableService;
		}

		public async Task<IHttpActionResult> Get()
		{
			GetLoadtestsForTimePeriodRequest getLoadtestsForTimePeriodRequest =
				new GetLoadtestsForTimePeriodRequest(DateTime.UtcNow, DateTime.UtcNow.AddDays(14));
			GetLoadtestsForTimePeriodResponse getLoadtestsForTimePeriodResponse =
				await _timetableService.GetLoadtestsForTimePeriodAsync(getLoadtestsForTimePeriodRequest);
			if (getLoadtestsForTimePeriodResponse.Exception == null)
			{
				return Ok<IEnumerable<LoadtestViewModel>>(getLoadtestsForTimePeriodResponse.Loadtests);
			}

			return InternalServerError(getLoadtestsForTimePeriodResponse.Exception);
		}

		public async Task<IHttpActionResult> Post(IEnumerable<InsertUpdateLoadtestViewModel> insertUpdateLoadtestViewModels)
		{
			List<LoadtestViewModel> loadtestViewModels = new List<LoadtestViewModel>();
			foreach (InsertUpdateLoadtestViewModel vm in insertUpdateLoadtestViewModels)
			{
				loadtestViewModels.Add(vm.ConvertToViewModel());
			}
			AddOrUpdateLoadtestsRequest request = new AddOrUpdateLoadtestsRequest(loadtestViewModels);
			AddOrUpdateLoadtestsResponse response = await _timetableService.AddOrUpdateLoadtestsAsync(request);
			if (response.Exception == null)
			{
				return Ok<string>(response.AddOrUpdateLoadtestsValidationResult.OperationResultSummary);
			}

			return InternalServerError(response.Exception);
		}

		public async Task<IHttpActionResult> Delete(Guid id)
		{
			DeleteLoadtestRequest request = new DeleteLoadtestRequest(id);
			DeleteLoadtestResponse response = await _timetableService.DeleteLoadtestAsync(request);
			if (response.Exception == null)
			{
				return Ok<string>("Deleted");
			}

			return InternalServerError(response.Exception);
		}
    }
}
