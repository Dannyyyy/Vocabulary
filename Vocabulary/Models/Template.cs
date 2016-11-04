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
        [Required]
        public string TemplateId { get; set; }
        [Required]
        [Display(Name = "Слово")]
        public string TemplateMessage { get; set; }
        [Display(Name = "Значение")]
        public string Description { get; set; }
    }
}