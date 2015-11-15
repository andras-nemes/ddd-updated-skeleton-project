using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using WebSuiteDDD.Infrastructure.Common.ApplicationSettings;
using WebSuiteDDD.SharedKernel.DomainEvents;
using WebSuiteDemo.Loadtesting.Domain;
using WebSuiteDemo.Loadtesting.Domain.DomainEvents;

namespace WebSuiteDemo.Loadtesting.ApplicationServices.Implementations
{
	public class TimetableChangedRabbitMqMessagingEventHandler : IDomainEventHandler
	{
		private readonly IConfigurationRepository _configurationRepository;

		public TimetableChangedRabbitMqMessagingEventHandler(IConfigurationRepository configurationRepository)
		{
			if (configurationRepository == null) throw new ArgumentNullException("ConfigurationRepository");
			_configurationRepository = configurationRepository;
		}

		public void Handle(EventArgs eventArgs)
		{
			TimetableChangedEventArgs e = eventArgs as TimetableChangedEventArgs;
			if (e != null)
			{
				AddOrUpdateLoadtestsValidationResult addOrUpdateValidationResult = e.AddOrUpdateLoadtestsValidationResult;
				if ((addOrUpdateValidationResult.ToBeInserted.Any() || addOrUpdateValidationResult.ToBeUpdated.Any())
						&& !addOrUpdateValidationResult.Failed.Any())
				{
					IConnection connection = GetRabbitMqConnection();
					IModel model = connection.CreateModel();
					string message = addOrUpdateValidationResult.OperationResultSummary;
					IBasicProperties basicProperties = model.CreateBasicProperties();
					byte[] messageBytes = Encoding.UTF8.GetBytes(message);
					string queue = _configurationRepository.GetConfigurationValue<string>("LoadtestEventMessageQueueName", "LoadtestEventMessageQueue");
					model.BasicPublish("", queue, basicProperties, messageBytes);
				}
			}
		}

		private IConnection GetRabbitMqConnection()
		{
			ConnectionFactory connectionFactory = new ConnectionFactory();
			connectionFactory.HostName = _configurationRepository.GetConfigurationValue<string>("LoadtestEventMessageQueueHost", "localhost");
			connectionFactory.UserName = _configurationRepository.GetConfigurationValue<string>("LoadtestEventMessageQueueUsername", "guest");
			connectionFactory.Password = _configurationRepository.GetConfigurationValue<string>("LoadtestEventMessageQueuePassword", "guest");
			return connectionFactory.CreateConnection();
		}
	}
}
