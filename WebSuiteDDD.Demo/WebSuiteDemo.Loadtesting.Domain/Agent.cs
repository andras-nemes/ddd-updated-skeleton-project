using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.SharedKernel;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public class Agent : EntityBase<Guid>
	{
		public Location Location { get; private set; }

		private Agent() : base(Guid.NewGuid()) { }

		public Agent(Guid id, string city, string country)
			: base(id)
		{
			Location = new Location(city, country);
		}
	}
}
