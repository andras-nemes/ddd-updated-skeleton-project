using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDemo.Loadtesting.ApplicationServices.Messaging
{
	public class DeleteLoadtestRequest : ServiceRequestBase
	{
		private Guid _id;

		public DeleteLoadtestRequest(Guid id)
		{
			_id = id;
		}

		public Guid Id
		{
			get
			{
				return _id;
			}
		}
	}
}
