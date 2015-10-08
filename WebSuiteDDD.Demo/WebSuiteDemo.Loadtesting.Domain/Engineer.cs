using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.SharedKernel;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public class Engineer : EntityBase<Guid>
	{
		private Engineer() : base(Guid.NewGuid()) { }

		public string Name { get; private set; }

		public Engineer(Guid guid, string name) : base(guid)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Engineer name");
			Name = name;
		}
	}
}
