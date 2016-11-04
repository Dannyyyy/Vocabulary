using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vocabulary.Models
{
    public class Template
    {
        // ID книги
        [Key]
        public string TemplateId { get; set; }
        // название книги
        public string TemplateMessage { get; set; }
        // автор книги
        public string Description { get; set; }
    }
}