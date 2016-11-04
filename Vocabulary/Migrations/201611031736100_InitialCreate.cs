namespace Vocabulary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        LanguageId = c.String(nullable: false, maxLength: 128),
                        LanguageDescription = c.String(),
                        Activity = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LanguageId);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        TemplateId = c.String(nullable: false, maxLength: 128),
                        TemplateMessage = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.TemplateId);
            
            CreateTable(
                "dbo.Translations",
                c => new
                    {
                        LanguageId = c.String(nullable: false, maxLength: 128),
                        MessageId = c.String(nullable: false, maxLength: 128),
                        MessageTranslation = c.String(),
                    })
                .PrimaryKey(t => new { t.LanguageId, t.MessageId });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Translations");
            DropTable("dbo.Templates");
            DropTable("dbo.Languages");
        }
    }
}
