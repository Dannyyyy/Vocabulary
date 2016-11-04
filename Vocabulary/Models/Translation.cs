using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vocabulary.Models
{
    public class Translation
    {
        [Key, Column(Order = 1)]
        public string LanguageId { get; set; }
        [Key, Column(Order = 2)]
        public string MessageId { get; set; }
        // автор книги
        public string MessageTranslation { get; set; }
    }
}