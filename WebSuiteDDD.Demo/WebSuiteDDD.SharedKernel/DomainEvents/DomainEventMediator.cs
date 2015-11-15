using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDDD.SharedKernel.DomainEvents
{
	public class DomainEventMediator
	{
		private static readonly DomainEventMediator _instance = new DomainEventMediator();
		private static List<IDomainEventHandler> _domainEventHandlers;

		private DomainEventMediator() { }

		public static DomainEventMediator Instance
		{
			get
			{
				return _instance;
			}
		}

		public static void RegisterDomainEventHandler(IDomainEventHandler domainEventHandler)
		{
			if (_domainEventHandlers == null)
			{
				_domainEventHandlers = new List<IDomainEventHandler>();
			}

			_domainEventHandlers.Add(domainEventHandler);
		}

		public static void RaiseEvent(EventArgs eventArgs)
		{
			if (_domainEventHandlers != null && _domainEventHandlers.Any())
			{
				foreach (var eventHandler in _domainEventHandlers)
				{
					eventHandler.Handle(eventArgs);
				}
			}
		}
	}
}
