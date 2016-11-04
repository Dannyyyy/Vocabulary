using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vocabulary.Models;

namespace Vocabulary.Controllers
{
    public class HomeController : Controller
    {
        VocabularyContext dbContext = new VocabularyContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ListLanguages()
        {
            var languages = dbContext.Languages;
            return View(languages.ToList());
        }

        [HttpGet]
        [Route("CreateLanguage")]
        public ActionResult CreateLanguage()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateLanguage")]
        public ActionResult CreateLanguage(Language language)
        {
            var langtag = language.LanguageId;
            try
            {
                CultureInfo lang = new CultureInfo(langtag);
                var vocabularyLanguage = dbContext.Languages.Find(langtag);
                if (vocabularyLanguage == null)
                {
                    dbContext.Languages.Add(language);
                    dbContext.SaveChanges();
                    return RedirectToAction("ListLanguages");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
            
        }

        [HttpGet]
        [Route("EditLanguage/{id}")]
        public ActionResult EditLanguage(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Language language = dbContext.Languages.Find(id);
            if(language != null)
            {
                return View(language);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Route("EditLanguage")]
        public ActionResult EditLanguage(Language language)
        {
            dbContext.Entry(language).State = EntityState.Modified;
            dbContext.SaveChanges();
            return RedirectToAction("ListLanguages");
        }
    }
}