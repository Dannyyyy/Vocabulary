using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vocabulary.Models
{
    public class PopularWord
    {
        // ID книги
        [Key]
        [Required]
        [Display(Name = "Слово")]
        public string WordId { get; set; }
        [Display(Name = "Значение")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Количество запросов")]
        public int Count { get; set; }
    }
}