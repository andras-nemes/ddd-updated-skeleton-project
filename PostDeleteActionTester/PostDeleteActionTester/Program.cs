using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PostDeleteActionTester
{
	class Program
	{
		private static Uri _serviceUri = new Uri("http://localhost:3522/loadtests");

		static void Main(string[] args)
		{
			//RunPostOperation();
			RunDeleteOperation();
		}

		private static void RunPostOperation()
		{
			HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, _serviceUri);
			requestMessage.Headers.ExpectContinue = false;
			List<InsertUpdateLoadtestViewModel> vms = new List<InsertUpdateLoadtestViewModel>();
			InsertUpdateLoadtestViewModel first = new InsertUpdateLoadtestViewModel()
			{
				AgentCity = "Seattle",
				AgentCountry = "USA",
				CustomerName = "OK Customer",
				DurationSec = 600,
				EngineerName = "Jane",
				LoadtestTypeShortDescription = "Stress test",
				ProjectName = "Third project",
				ScenarioUriOne = "http://www.hello.com",
				StartDate = new StartDate() { Year = 2015, Month = 8, Day = 22, Hour = 15, Minute = 30, Timezone = "E. Europe Standard Time" },
				UserCount = 30
			};
			InsertUpdateLoadtestViewModel second = new InsertUpdateLoadtestViewModel()
			{
				AgentCity = "Frankfurt",
				AgentCountry = "Germany",
				CustomerName = "Great customer",
				DurationSec = 20,
				EngineerName = "Fred",
				LoadtestTypeShortDescription = "Capacity test",
				ProjectName = "First project",
				ScenarioUriOne = "http://www.goodday.com",
				ScenarioUriTwo = "http://www.goodevening.com",
				StartDate = new StartDate() { Year = 2015, Month = 8, Day = 21, Hour = 16, Minute = 00, Timezone = "Nepal Standard Time" },
				UserCount = 50
			};

			vms.Add(first);
			vms.Add(second);

			string jsonInput = JsonConvert.SerializeObject(vms);
			requestMessage.Content = new StringContent(jsonInput, Encoding.UTF8, "application/json");
			HttpClient httpClient = new HttpClient();
			httpClient.Timeout = new TimeSpan(0, 10, 0);
			Task<HttpResponseMessage> httpRequest = httpClient.SendAsync(requestMessage,
					HttpCompletionOption.ResponseContentRead, CancellationToken.None);
			HttpResponseMessage httpResponse = httpRequest.Result;
			HttpStatusCode statusCode = httpResponse.StatusCode;
			HttpContent responseContent = httpResponse.Content;
			if (responseContent != null)
			{
				Task<String> stringContentsTask = responseContent.ReadAsStringAsync();
				String stringContents = stringContentsTask.Result;
				Console.WriteLine("Response from service: " + stringContents);
			}
			Console.ReadKey();
		}

		private static void RunDeleteOperation()
		{
			HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, string.Concat(_serviceUri, "/e3d4012c-50f6-4a58-af3a-5debfc40a01d"));
			requestMessage.Headers.ExpectContinue = false;
			HttpClient httpClient = new HttpClient();
			httpClient.Timeout = new TimeSpan(0, 10, 0);
			Task<HttpResponseMessage> httpRequest = httpClient.SendAsync(requestMessage,
					HttpCompletionOption.ResponseContentRead, CancellationToken.None);
			HttpResponseMessage httpResponse = httpRequest.Result;
			HttpStatusCode statusCode = httpResponse.StatusCode;
			HttpContent responseContent = httpResponse.Content;
			if (responseContent != null)
			{
				Task<String> stringContentsTask = responseContent.ReadAsStringAsync();
				String stringContents = stringContentsTask.Result;
				Console.WriteLine("Response from service: " + stringContents);
			}
			Console.ReadKey();
		}
	}
}
