namespace PipelineMLWeb.DataContexts.PipelineMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20170304Addingschemas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClassSchemas",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Classname = c.String(),
                        Schema = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClassSchemas");
        }
    }
}
