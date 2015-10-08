using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSuiteDemo.Loadtesting.Domain;

namespace WebSuiteDDD.WebApi.Models
{
	public class InsertUpdateLoadtestViewModel
	{
		public Guid Id { get; set; }
		public string AgentCity { get; set; }
		public string AgentCountry { get; set; }
		public string CustomerName { get; set; }
		public string EngineerName { get; set; }
		public string LoadtestTypeShortDescription { get; set; }
		public string ProjectName { get; set; }
		public string ScenarioUriOne { get; set; }
		public string ScenarioUriTwo { get; set; }
		public string ScenarioUriThree { get; set; }
		public StartDate StartDate { get; set; }
		public int UserCount { get; set; }
		public int DurationSec { get; set; }

		public LoadtestViewModel ConvertToViewModel()
		{
			LoadtestViewModel ltVm = new LoadtestViewModel();
			if (Id == null || Id == default(Guid)) Id = Guid.NewGuid();
			if (string.IsNullOrEmpty(AgentCity)) throw new ArgumentNullException("Agent city must be provided.");
			if (string.IsNullOrEmpty(AgentCountry)) throw new ArgumentNullException("Agent country must be provided.");
			if (string.IsNullOrEmpty(CustomerName)) throw new ArgumentNullException("Customer name must be provided.");
			if (string.IsNullOrEmpty(LoadtestTypeShortDescription)) throw new ArgumentNullException("Load test type must be provided.");
			if (string.IsNullOrEmpty(ProjectName)) throw new ArgumentNullException("Project name must be provided.");
			if (string.IsNullOrEmpty(ScenarioUriOne)) throw new ArgumentNullException("At least one URL must be provided for the scenario.");
			ltVm.AgentCity = AgentCity;
			ltVm.AgentCountry = AgentCountry;
			ltVm.CustomerName = CustomerName;
			ltVm.DurationSec = DurationSec;
			ltVm.EngineerName = EngineerName;
			ltVm.Id = Id;
			ltVm.LoadtestTypeShortDescription = LoadtestTypeShortDescription;
			ltVm.ProjectName = ProjectName;
			ltVm.ScenarioUriOne = ScenarioUriOne;
			ltVm.ScenarioUriTwo = ScenarioUriTwo;
			ltVm.ScenarioUriThree = ScenarioUriThree;

			DateTime customerDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, StartDate.Hour, StartDate.Minute, 0);
			TimeZoneInfo customerTimeZone = TimeZoneInfo.FindSystemTimeZoneById(StartDate.Timezone);
			ltVm.StartDateUtc = TimeZoneInfo.ConvertTimeToUtc(customerDate, customerTimeZone);
			ltVm.UserCount = UserCount;

			return ltVm;
		}
	}
}