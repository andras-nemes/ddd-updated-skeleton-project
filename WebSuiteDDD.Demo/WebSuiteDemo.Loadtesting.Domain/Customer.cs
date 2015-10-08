using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.SharedKernel;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public class Customer : EntityBase<Guid>
	{
		private Customer() : base(Guid.NewGuid()) { }

		public string Name { get; private set; }

		public Customer(Guid guid, string name) : base(guid)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("Customer name");
			Name = name;
		}
	}
}
