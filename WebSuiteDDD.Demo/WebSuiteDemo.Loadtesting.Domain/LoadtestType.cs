using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.SharedKernel;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public class LoadtestType : EntityBase<Guid>
	{
		private LoadtestType() : base(Guid.NewGuid()) { }

		public Description Description { get; private set; }

		public LoadtestType(Guid guid, string shortDescription, string longDescription)
			: base(guid)
		{
			Description = new Description(shortDescription, longDescription);
		}
	}
}
