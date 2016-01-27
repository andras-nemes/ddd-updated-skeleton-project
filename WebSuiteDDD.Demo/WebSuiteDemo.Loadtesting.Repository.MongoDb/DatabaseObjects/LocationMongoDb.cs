using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace WebSuiteDemo.Loadtesting.Repository.MongoDb.DatabaseObjects
{
	public class LocationMongoDb : MongoDbObjectBase
	{
		public string City { get; set; }
		public string Country { get; set; }
	}
}
