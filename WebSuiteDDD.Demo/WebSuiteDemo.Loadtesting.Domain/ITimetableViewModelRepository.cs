using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public interface ITimetableViewModelRepository
	{
		IList<LoadtestViewModel> ConvertToViewModels(IEnumerable<Loadtest> domains);
		IList<Loadtest> ConvertToDomain(IEnumerable<LoadtestViewModel> viewModels);
	}
}
