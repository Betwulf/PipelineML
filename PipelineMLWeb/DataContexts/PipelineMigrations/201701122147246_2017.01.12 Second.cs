namespace PipelineMLWeb.DataContexts.PipelineMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170112Second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PipelineResultsIds",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        RunDate = c.DateTime(nullable: false),
                        PipelineProject_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PipelineProjects", t => t.PipelineProject_Id)
                .Index(t => t.PipelineProject_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PipelineResultsIds", "PipelineProject_Id", "dbo.PipelineProjects");
            DropIndex("dbo.PipelineResultsIds", new[] { "PipelineProject_Id" });
            DropTable("dbo.PipelineResultsIds");
        }
    }
}
