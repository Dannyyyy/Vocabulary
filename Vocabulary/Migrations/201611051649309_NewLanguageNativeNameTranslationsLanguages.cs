namespace Vocabulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewLanguageNativeNameTranslationsLanguages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Languages", "LanguageNativeName", c => c.String());
            AddColumn("dbo.Translations", "LanguageNativeName", c => c.String());
            DropColumn("dbo.Languages", "LanguageLocalName");
            DropColumn("dbo.Translations", "LanguageLocalName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Translations", "LanguageLocalName", c => c.String());
            AddColumn("dbo.Languages", "LanguageLocalName", c => c.String());
            DropColumn("dbo.Translations", "LanguageNativeName");
            DropColumn("dbo.Languages", "LanguageNativeName");
        }
    }
}
