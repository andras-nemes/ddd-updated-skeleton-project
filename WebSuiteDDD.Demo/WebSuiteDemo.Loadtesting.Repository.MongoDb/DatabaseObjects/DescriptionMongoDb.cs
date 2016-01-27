using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace WebSuiteDemo.Loadtesting.Repository.MongoDb.DatabaseObjects
{
	public class DescriptionMongoDb : MongoDbObjectBase
	{
		public string ShortDescription { get; set; }
		public string LongDescription { get; set; }
	}
}
