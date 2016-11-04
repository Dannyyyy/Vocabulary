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

        [HttpGet]
        [Route("DeleteLanguage/{id}")]
        public ActionResult DeleteLanguage(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Language language = dbContext.Languages.Find(id);
            if (language != null)
            {
                return View(language);
            }
            return HttpNotFound();
        }

        [HttpPost, ActionName("DeleteLanguage")]
        public ActionResult DeleteLanguageConfirmed(string id)
        {
            Language language = dbContext.Languages.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            dbContext.Languages.Remove(language);
            dbContext.SaveChanges();
            return RedirectToAction("ListLanguages");
        }

        public ActionResult ListTemplates()
        {
            var templates = dbContext.Template;
            return View(templates.ToList());
        }

        [HttpGet]
        [Route("CreateTemplate")]
        public ActionResult CreateTemplate()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateLanguage")]
        public ActionResult CreateTemplate(Template template)
        {
            var vocabularyLanguage = dbContext.Languages.Find(template.TemplateMessage);
            if (vocabularyLanguage == null)
            {
                template.TemplateId = template.TemplateMessage;
                dbContext.Template.Add(template);
                dbContext.SaveChanges();
                return RedirectToAction("ListTemplates");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("EditTemplate/{id}")]
        public ActionResult EditTemplate(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Template template = dbContext.Template.Find(id);
            if (template != null)
            {
                return View(template);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Route("EditTemplate")]
        public ActionResult EditTemplate(Template template)
        {
            dbContext.Entry(template).State = EntityState.Modified;
            dbContext.SaveChanges();
            return RedirectToAction("ListTemplates");
        }

        [HttpGet]
        [Route("DeleteTemplate/{id}")]
        public ActionResult DeleteTemplate(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Template template = dbContext.Template.Find(id);
            if (template != null)
            {
                return View(template);
            }
            return HttpNotFound();
        }

        [HttpPost, ActionName("DeleteTemplate")]
        public ActionResult DeleteTemplateConfirmed(string id)
        {
            Template template = dbContext.Template.Find(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            dbContext.Template.Remove(template);
            dbContext.SaveChanges();
            return RedirectToAction("ListTemplates");
        }

        [Route("Translations")]
        public ActionResult Translations()
        {
            var languages = dbContext.Languages;
            return View(languages.ToList());
        }

        [HttpGet]
        [Route("Translation/{id}")]
        public ActionResult Translation(string id)
        {
            ViewBag.LanguageTitle = id;
            var templatesId = dbContext.Template.Select(t => t.TemplateId).ToList();
            var translationsId = dbContext.Translations.Where(t => t.LanguageId == id).Select(t => t.MessageId).ToList();
            foreach (var template in templatesId)
            {
                if (!translationsId.Contains(template))
                {
                    Translation translation = new Translation();
                    translation.LanguageId = id;
                    translation.MessageId = template;
                    translation.MessageTranslation = null;
                    dbContext.Translations.Add(translation);
                }
            }
            dbContext.SaveChanges();
            var translations = dbContext.Translations.Where(t => t.LanguageId == id).Select(t => t);
            return View(translations.ToList());
        }

        [HttpPost]
        [Route("Translation")]
        public ActionResult Translation(List<Translation> translations)
        {
            foreach (Translation translation in translations)
            {
                dbContext.Entry(translation).State = EntityState.Modified;  
            }
            dbContext.SaveChanges();
            return RedirectToAction("Translations");
        }
    }
}