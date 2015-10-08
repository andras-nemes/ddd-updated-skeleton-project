using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDDD.Repository.EF.DataModel
{
	public class Loadtest
	{
		public Guid Id { get; set; }
		public Guid AgentId { get; set; }
		public Guid CustomerId { get; set; }
		public Guid? EngineerId { get; set; }
		public Guid LoadtestTypeId { get; set; }
		public Guid ProjectId { get; set; }
		public Guid ScenarioId { get; set; }
		public LoadtestParameters Parameters { get; set; }
	}
}
