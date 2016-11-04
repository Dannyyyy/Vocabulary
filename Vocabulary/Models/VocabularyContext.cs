using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Vocabulary.Models
{
    public class VocabularyContext : DbContext
    {
        public DbSet<Template> Template { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Translation> Translations { get; set; }
    }
}