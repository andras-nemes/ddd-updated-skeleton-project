using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.SharedKernel;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public class Loadtest : EntityBase<Guid>
	{
		private Loadtest() : base(Guid.NewGuid()) { }

		public Guid AgentId { get; private set; }
		public Guid CustomerId { get; private set; }
		public Guid? EngineerId { get; private set; }
		public Guid LoadtestTypeId { get; private set; }
		public Guid ProjectId { get; private set; }
		public Guid ScenarioId { get; private set; }
		public LoadtestParameters Parameters { get; private set; }

		public Loadtest(Guid guid, LoadtestParameters parameters, Guid agentId, Guid customerId, Guid? engineerId
			, Guid loadtestTypeId, Guid projectId, Guid scenarioId)
			: base(guid)
		{
			RaiseIfDefaultGuid(guid);
			AssignParameters(parameters, agentId, customerId, engineerId, loadtestTypeId, projectId, scenarioId);
		}

		public void Update(LoadtestParameters parameters, Guid agentId, Guid customerId, Guid? engineerId
			, Guid loadtestTypeId, Guid projectId, Guid scenarioId)
		{
			AssignParameters(parameters, agentId, customerId, engineerId, loadtestTypeId, projectId, scenarioId);
		}

		private void AssignParameters(LoadtestParameters parameters, Guid agentId, Guid customerId, Guid? engineerId
			, Guid loadtestTypeId, Guid projectId, Guid scenarioId)
		{
			RaiseIfDefaultGuid(agentId);
			RaiseIfDefaultGuid(customerId);
			if (engineerId.HasValue) RaiseIfDefaultGuid(engineerId.Value);
			RaiseIfDefaultGuid(loadtestTypeId);
			RaiseIfDefaultGuid(projectId);
			RaiseIfDefaultGuid(scenarioId);
			AgentId = agentId;
			CustomerId = customerId;
			EngineerId = engineerId;
			LoadtestTypeId = loadtestTypeId;
			ProjectId = projectId;
			ScenarioId = scenarioId;
			Parameters = parameters;
		}

		private void RaiseIfDefaultGuid(Guid guid)
		{
			if (guid == default(Guid))
			{
				throw new ArgumentException("Default GUID not acceptable");
			}
		}
	}
}
