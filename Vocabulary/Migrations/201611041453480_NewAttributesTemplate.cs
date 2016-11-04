namespace Vocabulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewAttributesTemplate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Templates", "TemplateMessage", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Templates", "TemplateMessage", c => c.String());
        }
    }
}
