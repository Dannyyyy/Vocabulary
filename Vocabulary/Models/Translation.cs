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
        [Display(Name = "Аббревиатура языка")]
        public string LanguageId { get; set; }
        [Key, Column(Order = 2)]
        public string MessageId { get; set; }
        // автор книги
        [Display(Name = "Перевод")]
        public string MessageTranslation { get; set; }
        [Display(Name = "Естественное имя языка")]
        public string LanguageNativeName { get; set; }
    }
}
