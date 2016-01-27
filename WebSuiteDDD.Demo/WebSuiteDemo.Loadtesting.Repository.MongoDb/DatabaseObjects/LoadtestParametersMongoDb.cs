using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace WebSuiteDemo.Loadtesting.Repository.MongoDb.DatabaseObjects
{
	public class LoadtestParametersMongoDb : MongoDbObjectBase
	{
		public DateTime StartDateUtc { get; set; }
		public int UserCount { get; set; }
		public int DurationSec { get; set; }
		public DateTime ExpectedEndDateUtc { get; set; }
	}
}
