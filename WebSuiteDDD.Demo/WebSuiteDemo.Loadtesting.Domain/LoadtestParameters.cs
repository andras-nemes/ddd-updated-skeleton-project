using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.SharedKernel;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public class LoadtestParameters : ValueObjectBase<LoadtestParameters>
	{
		private LoadtestParameters() { }

		public DateTime StartDateUtc { get; private set; }
		public int UserCount { get; private set; }
		public int DurationSec { get; private set; }

		public LoadtestParameters(DateTime startDateUtc, int userCount, int durationSec)
		{
			if (userCount < 1) throw new ArgumentException("User count cannot be less than 1");
			if (durationSec < 30) throw new ArgumentException("Test duration cannot be less than 30 seconds.");
			if (durationSec > 3600) throw new ArgumentException("Test duration cannot be more than 3600 seconds, i.e. 1 hour.");
			if (startDateUtc < DateTime.UtcNow) startDateUtc = DateTime.UtcNow;
			StartDateUtc = startDateUtc;
			UserCount = userCount;
			DurationSec = durationSec;
		}

		public DateTime GetEndDateUtc()
		{
			return StartDateUtc.AddSeconds(DurationSec);
		}

		public LoadtestParameters WithStartDateUtc(DateTime newStartDate)
		{
			return new LoadtestParameters(newStartDate, UserCount, DurationSec);
		}

		public LoadtestParameters WithUserCount(int userCount)
		{
			return new LoadtestParameters(StartDateUtc, userCount, DurationSec);
		}

		public LoadtestParameters WithDuration(int durationSec)
		{
			return new LoadtestParameters(StartDateUtc, UserCount, durationSec);
		}

		public override bool Equals(LoadtestParameters other)
		{
			return other.UserCount == this.UserCount && other.StartDateUtc == this.StartDateUtc && other.DurationSec == this.DurationSec;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (!(obj is LoadtestParameters)) return false;
			return this.Equals((LoadtestParameters)obj);
		}

		public override int GetHashCode()
		{
			return this.DurationSec.GetHashCode() + this.StartDateUtc.GetHashCode() + this.UserCount.GetHashCode();
		}
	}
}
