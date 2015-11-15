using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDDD.Infrastructure.Common.Emailing
{
	public class EmailArguments
	{
		private string _subject;
		private string _message;
		private string _to;
		private string _from;
		private string _smtpServer;

		public EmailArguments(string subject, string message, string to, string from, string smtpServer)
		{
			if (string.IsNullOrEmpty(subject))
				throw new ArgumentNullException("Email subject");
			if (string.IsNullOrEmpty(message))
				throw new ArgumentNullException("Email message");
			if (string.IsNullOrEmpty(to))
				throw new ArgumentNullException("Email recipient");
			if (string.IsNullOrEmpty(from))
				throw new ArgumentNullException("Email sender");
			if (string.IsNullOrEmpty(smtpServer))
				throw new ArgumentNullException("Smtp server");
			this._from = from;
			this._message = message;
			this._smtpServer = smtpServer;
			this._subject = subject;
			this._to = to;
		}

		public string To
		{
			get
			{
				return this._to;
			}
		}

		public string From
		{
			get
			{
				return this._from;
			}
		}

		public string Subject
		{
			get
			{
				return this._subject;
			}
		}

		public string SmtpServer
		{
			get
			{
				return this._smtpServer;
			}
		}

		public string Message
		{
			get
			{
				return this._message;
			}
		}
	}
}
