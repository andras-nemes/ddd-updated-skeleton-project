using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDemo.Loadtesting.Domain;

namespace WebSuiteDemo.Loadtesting.ApplicationServices.Messaging
{
	public class AddOrUpdateLoadtestsResponse : ServiceResponseBase
	{
		public AddOrUpdateLoadtestsValidationResult AddOrUpdateLoadtestsValidationResult { get; set; }
	}
}
