namespace Vocabulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewLanguageLocalNameTranslationsLanguages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Languages", "LanguageLocalName", c => c.String());
            AddColumn("dbo.Translations", "LanguageLocalName", c => c.String());
            DropColumn("dbo.Languages", "LocalName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Languages", "LocalName", c => c.String());
            DropColumn("dbo.Translations", "LanguageLocalName");
            DropColumn("dbo.Languages", "LanguageLocalName");
        }
    }
}
