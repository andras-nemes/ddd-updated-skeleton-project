using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WebSuiteDDD.FinancialPlanner
{
	class Program
	{
		private static string _hostName = "localhost";
		private static string _userName = "guest";
		private static string _password = "guest";
		private static string _loadtestEventMessageQueueName = "LoadtestEventMessageQueue";
		
		static void Main(string[] args)
		{
			//BuildMessageQueue();
			ReceiveMessages();
		}

		private static void BuildMessageQueue()
		{
			IConnection connection = GetRabbitMqConnection();
			IModel model = connection.CreateModel();
			model.QueueDeclare(_loadtestEventMessageQueueName, true, false, false, null);
		}

		private static void ReceiveMessages()
		{
			IConnection connection = GetRabbitMqConnection();
			IModel model = connection.CreateModel();
			model.BasicQos(0, 1, false); //basic quality of service
			QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
			model.BasicConsume(_loadtestEventMessageQueueName, false, consumer);
			while (true)
			{
				BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
				String message = Encoding.UTF8.GetString(deliveryArguments.Body);
				Console.WriteLine("Message received: {0}", message);
				model.BasicAck(deliveryArguments.DeliveryTag, false);
			}
		}

		private static IConnection GetRabbitMqConnection()
		{
			ConnectionFactory connectionFactory = new ConnectionFactory();
			connectionFactory.HostName = _hostName;
			connectionFactory.UserName = _userName;
			connectionFactory.Password = _password;
			return connectionFactory.CreateConnection();
		}
	}
}
