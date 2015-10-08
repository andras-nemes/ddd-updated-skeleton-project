using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDemo.Loadtesting.Domain;

namespace WebSuiteDemo.Loadtesting.Repository.EF.Repositories
{
	public class TimetableViewModelRepository : ITimetableViewModelRepository
	{
		public IList<LoadtestViewModel> ConvertToViewModels(IEnumerable<Loadtest> domains)
		{
			LoadTestingContext context = new LoadTestingContext();
			List<LoadtestViewModel> viewModels = new List<LoadtestViewModel>();
			foreach (Loadtest lt in domains)
			{
				LoadtestViewModel vm = new LoadtestViewModel();
				vm.Id = lt.Id;
				Agent agent = (from a in context.Agents where a.Id == lt.AgentId select a).FirstOrDefault();
				if (agent == null) throw new ArgumentException("There is no load test agent with the given ID.");
				vm.AgentCountry = agent.Location.Country;
				vm.AgentCity = agent.Location.City;

				Customer customer = (from c in context.Customers where c.Id == lt.CustomerId select c).FirstOrDefault();
				if (customer == null) throw new ArgumentException("There is no customer with the given ID.");
				vm.CustomerName = customer.Name;

				if (lt.EngineerId.HasValue)
				{
					Engineer engineer = (from e in context.Engineers where e.Id == lt.EngineerId.Value select e).FirstOrDefault();
					if (engineer == null) throw new ArgumentException("There is no engineer with the given ID.");
					vm.EngineerName = engineer.Name;
				}

				LoadtestType loadtestType = (from t in context.LoadtestTypes where t.Id == lt.LoadtestTypeId select t).FirstOrDefault();
				if (loadtestType == null) throw new ArgumentException("There is no load test type with the given ID.");
				vm.LoadtestTypeShortDescription = loadtestType.Description.ShortDescription;

				Project project = (from p in context.Projects where p.Id == lt.ProjectId select p).FirstOrDefault();
				if (project == null) throw new ArgumentException("There is no project with the given ID.");
				vm.ProjectName = project.Description.ShortDescription;

				Scenario scenario = (from s in context.Scenarios where s.Id == lt.ScenarioId select s).FirstOrDefault();
				if (scenario == null) throw new ArgumentException("There is no scenario with the given ID.");
				vm.ScenarioUriOne = scenario.UriOne;
				vm.ScenarioUriTwo = scenario.UriTwo;
				vm.ScenarioUriThree = scenario.UriThree;

				vm.UserCount = lt.Parameters.UserCount;
				vm.StartDateUtc = lt.Parameters.StartDateUtc;
				vm.DurationSec = lt.Parameters.DurationSec;

				viewModels.Add(vm);
			}
			return viewModels;
		}

		public IList<Loadtest> ConvertToDomain(IEnumerable<LoadtestViewModel> viewModels)
		{
			List<Loadtest> loadtests = new List<Loadtest>();
			LoadTestingContext context = new LoadTestingContext();
			foreach (LoadtestViewModel vm in viewModels)
			{
				Guid id = vm.Id;
				LoadtestParameters ltParams = new LoadtestParameters(vm.StartDateUtc, vm.UserCount, vm.DurationSec);
				Agent agent = (from a in context.Agents
							   where a.Location.City.Equals(vm.AgentCity, StringComparison.InvariantCultureIgnoreCase)
								   && a.Location.Country.ToLower() == vm.AgentCountry.ToLower()
							   select a).FirstOrDefault();
				if (agent == null) throw new ArgumentException("There is no agent with the given properties.");

				Customer customer = (from c in context.Customers where c.Name.Equals(vm.CustomerName, StringComparison.InvariantCultureIgnoreCase) select c).FirstOrDefault();
				if (customer == null) throw new ArgumentException("There is no customer with the given properties.");

				Guid? engineerId = null;
				if (!string.IsNullOrEmpty(vm.EngineerName))
				{
					Engineer engineer = (from e in context.Engineers where e.Name.Equals(vm.EngineerName, StringComparison.InvariantCultureIgnoreCase) select e).FirstOrDefault();
					if (engineer == null) throw new ArgumentException("There is no engineer with the given properties.");
					engineerId = engineer.Id;
				}

				LoadtestType ltType = (from t in context.LoadtestTypes where t.Description.ShortDescription.Equals(vm.LoadtestTypeShortDescription, StringComparison.InvariantCultureIgnoreCase) select t).FirstOrDefault();
				if (ltType == null) throw new ArgumentException("There is no load test type with the given properties.");

				Project project = (from p in context.Projects where p.Description.ShortDescription.ToLower() == vm.ProjectName.ToLower() select p).FirstOrDefault();
				if (project == null) throw new ArgumentException("There is no project with the given properties.");

				Scenario scenario = (from s in context.Scenarios
									 where s.UriOne.Equals(vm.ScenarioUriOne, StringComparison.InvariantCultureIgnoreCase)
										 && s.UriTwo.Equals(vm.ScenarioUriTwo, StringComparison.InvariantCultureIgnoreCase)
										 && s.UriThree.Equals(vm.ScenarioUriThree, StringComparison.InvariantCultureIgnoreCase)
									 select s).FirstOrDefault();

				if (scenario == null)
				{
					List<Uri> uris = new List<Uri>();
					Uri firstUri = string.IsNullOrEmpty(vm.ScenarioUriOne) ? null : new Uri(vm.ScenarioUriOne);
					Uri secondUri = string.IsNullOrEmpty(vm.ScenarioUriTwo) ? null : new Uri(vm.ScenarioUriTwo);
					Uri thirdUri = string.IsNullOrEmpty(vm.ScenarioUriThree) ? null : new Uri(vm.ScenarioUriThree);
					if (firstUri != null) uris.Add(firstUri);
					if (secondUri != null) uris.Add(secondUri);
					if (thirdUri != null) uris.Add(thirdUri);
					scenario = new Scenario(Guid.NewGuid(), uris);
					context.Scenarios.Add(scenario);
					context.SaveChanges();
				}

				Loadtest converted = new Loadtest(id, ltParams, agent.Id, customer.Id, engineerId, ltType.Id, project.Id, scenario.Id);
				loadtests.Add(converted);
			}
			return loadtests;
		}
	}
}
