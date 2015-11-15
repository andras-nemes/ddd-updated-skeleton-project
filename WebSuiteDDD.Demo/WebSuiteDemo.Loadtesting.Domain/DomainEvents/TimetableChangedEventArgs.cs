using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDemo.Loadtesting.Domain.DomainEvents
{
	public class TimetableChangedEventArgs : EventArgs
	{
		private AddOrUpdateLoadtestsValidationResult _addOrUpdateLoadtestsValidationResult;

		public TimetableChangedEventArgs(AddOrUpdateLoadtestsValidationResult addOrUpdateLoadtestsValidationResult)
		{
			if (addOrUpdateLoadtestsValidationResult == null) throw new ArgumentNullException("AddOrUpdateLoadtestsValidationResult");
			_addOrUpdateLoadtestsValidationResult = addOrUpdateLoadtestsValidationResult;
		}

		public AddOrUpdateLoadtestsValidationResult AddOrUpdateLoadtestsValidationResult
		{
			get
			{
				return _addOrUpdateLoadtestsValidationResult;
			}
		}
	}
}
