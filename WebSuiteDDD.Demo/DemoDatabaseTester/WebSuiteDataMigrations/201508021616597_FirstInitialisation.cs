namespace DemoDatabaseTester.WebSuiteDataMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstInitialisation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
						Location_City = c.String(nullable: false),
						Location_Country = c.String(nullable: false),
                        Location_Longitude = c.Double(nullable: false),
                        Location_Latitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
						Name = c.String(nullable: false),
						Address = c.String(nullable: false),
						MainContact = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Engineers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
						Name = c.String(nullable: false),
						Title = c.String(nullable: false),
                        YearJoinedCompany = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Loadtests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AgentId = c.Guid(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        EngineerId = c.Guid(),
                        LoadtestTypeId = c.Guid(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                        ScenarioId = c.Guid(nullable: false),
                        Parameters_StartDateUtc = c.DateTime(nullable: false),
                        Parameters_UserCount = c.Int(nullable: false),
                        Parameters_DurationSec = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoadtestTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
						Description_ShortDescription = c.String(nullable: false),
						Description_LongDescription = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
						Description_ShortDescription = c.String(nullable: false),
						Description_LongDescription = c.String(nullable: false),
                        DateInsertedUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Scenarios",
                c => new
                    {
                        Id = c.Guid(nullable: false),
						UriOne = c.String(nullable: false),
                        UriTwo = c.String(),
                        UriThree = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Scenarios");
            DropTable("dbo.Projects");
            DropTable("dbo.LoadtestTypes");
            DropTable("dbo.Loadtests");
            DropTable("dbo.Engineers");
            DropTable("dbo.Customers");
            DropTable("dbo.Agents");
        }
    }
}
