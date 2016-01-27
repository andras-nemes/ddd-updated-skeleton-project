using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebSuiteDemo.Loadtesting.Repository.MongoDb.DatabaseObjects
{
	public class LoadtestMongoDb : MongoDbDomainBase
	{
		[BsonRepresentation(BsonType.String)]
		public Guid AgentId { get; set; }
		[BsonRepresentation(BsonType.String)]
		public Guid CustomerId { get; set; }
		[BsonRepresentation(BsonType.String)]
		public Guid? EngineerId { get; set; }
		[BsonRepresentation(BsonType.String)]
		public Guid LoadtestTypeId { get; set; }
		[BsonRepresentation(BsonType.String)]
		public Guid ProjectId { get; set; }
		[BsonRepresentation(BsonType.String)]
		public Guid ScenarioId { get; set; }
		public LoadtestParametersMongoDb Parameters { get; set; }
	}
}
