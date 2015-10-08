using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public class LoadtestValidationSummary
	{
		public bool OkToAddOrModify { get; set; }
		public string ReasonForValidationFailure { get; set; }
	}
}
