using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDDD.SharedKernel.DomainEvents
{
	public interface IDomainEventHandler
	{
		void Handle(EventArgs eventArgs);
	}
}
