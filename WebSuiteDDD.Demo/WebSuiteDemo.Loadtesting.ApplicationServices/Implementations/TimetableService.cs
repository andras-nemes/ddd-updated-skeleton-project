using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDemo.Loadtesting.ApplicationServices.Abstractions;
using WebSuiteDemo.Loadtesting.ApplicationServices.Messaging;
using WebSuiteDemo.Loadtesting.Domain;

namespace WebSuiteDemo.Loadtesting.ApplicationServices.Implementations
{
	public class TimetableService : ITimetableService
	{
		private readonly ITimetableRepository _timetableRepository;
		private readonly ITimetableViewModelRepository _timetableViewModelRepository;

		public TimetableService(ITimetableRepository timetableRepository, ITimetableViewModelRepository timetableViewModelRepository)
		{
			if (timetableRepository == null) throw new ArgumentNullException("TimetableRepository");
			if (timetableViewModelRepository == null) throw new ArgumentNullException("TimetableViewModelRepository");
			_timetableRepository = timetableRepository;
			_timetableViewModelRepository = timetableViewModelRepository;
		}

		public async Task<AddOrUpdateLoadtestsResponse> AddOrUpdateLoadtestsAsync(AddOrUpdateLoadtestsRequest addOrUpdateLoadtestsRequest)
		{
			return await Task<AddOrUpdateLoadtestsResponse>.Run(() => AddOrUpdateLoadtests(addOrUpdateLoadtestsRequest));
		}

		public async Task<DeleteLoadtestResponse> DeleteLoadtestAsync(DeleteLoadtestRequest deleteLoadtestRequest)
		{
			return await Task<DeleteLoadtestResponse>.Run(() => DeleteLoadtest(deleteLoadtestRequest));
		}

		public async Task<GetLoadtestsForTimePeriodResponse> GetLoadtestsForTimePeriodAsync(GetLoadtestsForTimePeriodRequest getLoadtestsForTimePeriodRequest)
		{
			return await Task<GetLoadtestsForTimePeriodResponse>.Run(() => GetLoadtestsForTimePeriod(getLoadtestsForTimePeriodRequest));
		}

		private AddOrUpdateLoadtestsResponse AddOrUpdateLoadtests(AddOrUpdateLoadtestsRequest addOrUpdateLoadtestsRequest)
		{
			AddOrUpdateLoadtestsResponse resp = new AddOrUpdateLoadtestsResponse();
			try
			{
				//assign ID if not present, assume to be insertion
				foreach (LoadtestViewModel ltvm in addOrUpdateLoadtestsRequest.Loadtests)
				{
					if (ltvm.Id == null || ltvm.Id == default(Guid))
					{
						ltvm.Id = Guid.NewGuid();
					}
				}
				List<LoadtestViewModel> sortedByDate = addOrUpdateLoadtestsRequest.Loadtests.OrderBy(lt => lt.StartDateUtc).ToList();
				LoadtestViewModel last = sortedByDate.Last();
				IList<Loadtest> loadtests = _timetableRepository.GetLoadtestsForTimePeriod(sortedByDate.First().StartDateUtc, last.StartDateUtc.AddSeconds(last.DurationSec));
				Timetable timetable = new Timetable(loadtests);				
				IList<Loadtest> loadtestsAddedOrUpdated = _timetableViewModelRepository.ConvertToDomain(addOrUpdateLoadtestsRequest.Loadtests);
				AddOrUpdateLoadtestsValidationResult validationResult = timetable.AddOrUpdateLoadtests(loadtestsAddedOrUpdated);
				_timetableRepository.AddOrUpdateLoadtests(validationResult);
				resp.AddOrUpdateLoadtestsValidationResult = validationResult;
			}
			catch (Exception ex)
			{
				resp.Exception = ex;
			}
			return resp;
		}

		private DeleteLoadtestResponse DeleteLoadtest(DeleteLoadtestRequest deleteLoadtestRequest)
		{
			DeleteLoadtestResponse resp = new DeleteLoadtestResponse();
			try
			{
				_timetableRepository.DeleteById(deleteLoadtestRequest.Id);
			}
			catch (Exception ex)
			{
				resp.Exception = ex;
			}
			return resp;
		}

		private GetLoadtestsForTimePeriodResponse GetLoadtestsForTimePeriod(GetLoadtestsForTimePeriodRequest getLoadtestsForTimePeriodRequest)
		{
			GetLoadtestsForTimePeriodResponse resp = new GetLoadtestsForTimePeriodResponse();
			try
			{
				IList<Loadtest> loadtests = _timetableRepository.GetLoadtestsForTimePeriod(getLoadtestsForTimePeriodRequest.SearchStartDateUtc, getLoadtestsForTimePeriodRequest.SearchEndDateUtc);
				IEnumerable<LoadtestViewModel> ltVms = _timetableViewModelRepository.ConvertToViewModels(loadtests);
				resp.Loadtests = ltVms;
			}
			catch (Exception ex)
			{
				resp.Exception = ex;
			}
			return resp;
		}
	}
}
