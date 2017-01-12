namespace PipelineMLWeb.DataContexts.PipelineMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170112First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PipelineProjects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 40),
                        Description = c.String(maxLength: 200),
                        PipelineDefinitionGuid = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PipelineProjects");
        }
    }
}
