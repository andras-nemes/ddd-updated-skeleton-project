using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.SharedKernel;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public class Timetable : IAggregateRoot
	{
		public IList<Loadtest> Loadtests { get; private set; }

		public Timetable(IList<Loadtest> loadtests)
		{
			if (loadtests == null) loadtests = new List<Loadtest>();
			Loadtests = loadtests;
		}

		public AddOrUpdateLoadtestsValidationResult AddOrUpdateLoadtests(IList<Loadtest> loadtestsAddedOrUpdated)
		{
			List<Loadtest> toBeInserted = new List<Loadtest>();
			List<Loadtest> toBeUpdated = new List<Loadtest>();
			List<Loadtest> failed = new List<Loadtest>();
			StringBuilder resultSummaryBuilder = new StringBuilder();
			string NL = Environment.NewLine;
			foreach (Loadtest loadtest in loadtestsAddedOrUpdated)
			{
				Loadtest existing = (from l in Loadtests where l.Id == loadtest.Id select l).FirstOrDefault();
				if (existing != null) //update
				{
					LoadtestValidationSummary validationSummary = OkToAddOrModify(loadtest);
					if (validationSummary.OkToAddOrModify)
					{
						existing.Update
							(loadtest.Parameters, loadtest.AgentId, loadtest.CustomerId,
								loadtest.EngineerId, loadtest.LoadtestTypeId, loadtest.ProjectId, loadtest.ScenarioId);
						toBeUpdated.Add(existing);
						resultSummaryBuilder.Append(string.Format("Load test ID {0} (update) successfully validated.{1}", existing.Id, NL));
					}
					else
					{
						failed.Add(loadtest);
						resultSummaryBuilder.Append(string.Format("Load test ID {0} (update) validation failed: {1}{2}.",
							existing.Id, validationSummary.ReasonForValidationFailure, NL));
					}
				}
				else //insertion
				{
					LoadtestValidationSummary validationSummary = OkToAddOrModify(loadtest);
					if (validationSummary.OkToAddOrModify)
					{
						Loadtests.Add(loadtest);
						toBeInserted.Add(loadtest);
						resultSummaryBuilder.Append(string.Format("Load test ID {0} (insertion) successfully validated.{1}", loadtest.Id, NL));
					}
					else
					{
						failed.Add(loadtest);
						resultSummaryBuilder.Append(string.Format("Load test ID {0} (insertion) validation failed: {1}{2}.",
							loadtest.Id, validationSummary.ReasonForValidationFailure, NL));
					}
				}
			}

			return new AddOrUpdateLoadtestsValidationResult(toBeInserted, toBeUpdated, failed, resultSummaryBuilder.ToString());
		}

		private LoadtestValidationSummary OkToAddOrModify(Loadtest loadtest)
		{
			LoadtestValidationSummary validationSummary = new LoadtestValidationSummary();
			validationSummary.OkToAddOrModify = true;
			validationSummary.ReasonForValidationFailure = string.Empty;
			List<Loadtest> loadtestsOnSameAgent = (from l in Loadtests
												   where l.AgentId == loadtest.AgentId
												   && DatesOverlap(l, loadtest)
												   select l).ToList();
			if (loadtestsOnSameAgent.Count >= 2)
			{
				validationSummary.OkToAddOrModify = false;
				validationSummary.ReasonForValidationFailure += " The selected load test agent is already booked for this period. ";
			}

			if (loadtest.EngineerId.HasValue)
			{
				List<Loadtest> loadtestsOnSameEngineer = (from l in Loadtests
														  where loadtest.EngineerId.HasValue &&
														  l.EngineerId.Value == loadtest.EngineerId.Value
														  && DatesOverlap(l, loadtest)
														  select l).ToList();
				if (loadtestsOnSameEngineer.Any())
				{
					validationSummary.OkToAddOrModify = false;
					validationSummary.ReasonForValidationFailure += " The selected load test engineer is already booked for this period. ";
				}
			}

			return validationSummary;
		}

		private bool DatesOverlap(Loadtest loadtestOne, Loadtest loadtestTwo)
		{
			return (loadtestOne.Parameters.StartDateUtc < loadtestTwo.Parameters.GetEndDateUtc()
				&& loadtestTwo.Parameters.StartDateUtc < loadtestOne.Parameters.GetEndDateUtc());
		}
	}
}
