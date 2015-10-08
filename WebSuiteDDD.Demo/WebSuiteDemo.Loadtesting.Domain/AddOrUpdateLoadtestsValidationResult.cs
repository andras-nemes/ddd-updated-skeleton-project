using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public class AddOrUpdateLoadtestsValidationResult
	{
		public List<Loadtest> ToBeInserted { get; private set; }
		public List<Loadtest> ToBeUpdated { get; private set; }
		public List<Loadtest> Failed { get; private set; }
		public string OperationResultSummary { get; private set; }
		public bool ValidationComplete { get; private set; }

		public AddOrUpdateLoadtestsValidationResult(List<Loadtest> toBeInserted, List<Loadtest> toBeUpdated, 
			List<Loadtest> failed, string operationResultSummary)
		{			
			ToBeInserted = toBeInserted;
			ToBeUpdated = toBeUpdated;
			Failed = failed;
			OperationResultSummary = operationResultSummary;
			ValidationComplete = (toBeInserted != null && toBeUpdated != null && failed != null && !string.IsNullOrEmpty(operationResultSummary));
		}
	}
}
