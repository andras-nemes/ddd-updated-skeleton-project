using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDemo.Loadtesting.Domain;
using WebSuiteDemo.Loadtesting.Repository.EF;
using System.Diagnostics;
using WebSuiteDemo.Loadtesting.Repository.EF.Repositories;

namespace DemoDatabaseTester
{
	public class LoadtestingContextService
	{
		public void TestLoadtestingContext()
		{
			/*
			ITimetableRepository timetableRepo = new TimetableRepository();
			IList<Loadtest> loadtests = timetableRepo.GetLoadtestsForTimePeriod(DateTime.UtcNow.AddDays(-10),
				DateTime.UtcNow.AddDays(10));

			Timetable tt = new Timetable(loadtests);
			Loadtest newLoadtest = new Loadtest(Guid.Parse("8c928a5e-d038-44f3-a8ff-70f64a651155"),
				new LoadtestParameters(DateTime.UtcNow.AddDays(3), 120, 900), Guid.Parse("751ec485-437d-4bae-9ff1-1923203a87b1")
				, Guid.Parse("99f4dc94-718c-450d-87b6-3153bb8db622"), Guid.Parse("471119e2-2b3c-4545-97a2-5f52d1fa7954")
				, Guid.Parse("a868a7c5-2f4a-43f7-9a8c-a597793fdc56"), Guid.Parse("96877388-ce4d-4ea8-ae93-438a696386b9")
				, Guid.Parse("73e25716-7622-4af6-99a0-0638efb1c8cc"));
			List<Loadtest> allChanges = new List<Loadtest>() { newLoadtest };
			AddOrUpdateLoadtestsValidationResult res = tt.AddOrUpdateLoadtests(allChanges);
			Debug.WriteLine(res.OperationResultSummary);
			timetableRepo.AddOrUpdateLoadtests(res);

			timetableRepo.DeleteById(Guid.Parse("4e880392-5497-4c9e-a3de-38f66348fe8e"));*/

			
			ITimetableRepository timetableRepo = new TimetableRepository();
			ITimetableViewModelRepository viewModelRepo = new TimetableViewModelRepository();
			/*
			IList<Loadtest> loadtests = timetableRepo.GetLoadtestsForTimePeriod(DateTime.UtcNow.AddDays(-10),
				DateTime.UtcNow.AddDays(10));			
			IList<LoadtestViewModel> vms = viewModelRepo.ConvertToViewModels(loadtests);*/

			LoadtestViewModel vm = new LoadtestViewModel();
			vm.Id = Guid.NewGuid();
			vm.AgentCity = "Tokyo";
			vm.AgentCountry = "Japan";
			vm.CustomerName = "Great customer";
			vm.DurationSec = 300;
			vm.EngineerName = "Fred";
			vm.LoadtestTypeShortDescription = "stress test";
			vm.ProjectName = "Second project";
			vm.ScenarioUriOne = "http://www.hello.com";
			vm.ScenarioUriTwo = "http://www.seeyou.com";
			vm.StartDateUtc = DateTime.UtcNow.AddDays(2);
			vm.UserCount = 60;
			IList<LoadtestViewModel> vms = new List<LoadtestViewModel>() { vm };
			IList<Loadtest> loadtests = viewModelRepo.ConvertToDomain(vms);
		}
	}
}
