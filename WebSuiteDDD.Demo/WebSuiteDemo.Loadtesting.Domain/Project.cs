using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.SharedKernel;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public class Project : EntityBase<Guid>
	{
		private Project() : base(Guid.NewGuid()) { }

		public Description Description { get; private set; }

		public Project(Guid id, string shortDescription, string longDescription) : base(id)
		{			
			Description = new Description(shortDescription, longDescription);
		}
	}
}
