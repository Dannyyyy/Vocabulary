namespace Vocabulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewLocalNameLanguage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Languages", "LocalName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Languages", "LocalName");
        }
    }
}
