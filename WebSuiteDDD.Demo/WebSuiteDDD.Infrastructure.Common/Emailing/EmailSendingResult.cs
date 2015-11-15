using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDDD.Infrastructure.Common.Emailing
{
	public class EmailSendingResult
	{
		public bool EmailSentSuccessfully { get; set; }
		public string EmailSendingFailureMessage { get; set; }
	}
}
