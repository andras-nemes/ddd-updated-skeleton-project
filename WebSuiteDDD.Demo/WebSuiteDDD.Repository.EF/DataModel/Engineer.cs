using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDDD.Repository.EF.DataModel
{
	public class Engineer
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public int YearJoinedCompany { get; set; }
	}
}
