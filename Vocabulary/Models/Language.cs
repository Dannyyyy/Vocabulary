using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vocabulary.Models
{
    public class Language
    {
        // ID книги
        [Key]
        [Required]
        [Display(Name = "Аббревиатура")]
        public string LanguageId { get; set; }
        // название книги
        [Required]
        [Display(Name = "Описание")]
        public string LanguageDescription { get; set; }
        // автор книги
        [Required]
        [Display(Name = "Активность")]
        public bool Activity { get; set; }
    }
}