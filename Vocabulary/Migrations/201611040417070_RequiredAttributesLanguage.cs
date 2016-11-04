namespace Vocabulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredAttributesLanguage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Languages", "LanguageDescription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Languages", "LanguageDescription", c => c.String());
        }
    }
}
