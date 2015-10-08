using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDDD.Repository.EF.DataModel
{
	public class Agent
	{
		public Guid Id { get; set; }
		public Location Location { get; set; }
	}
}
