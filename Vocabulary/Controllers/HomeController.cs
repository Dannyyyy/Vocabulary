using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
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
                    language.LanguageNativeName = lang.NativeName;
                    dbContext.Languages.Add(language);
                    dbContext.SaveChanges();
                    Session["LanguageMessage"] = "Язык успешно добавлен в словарь.";
                    return RedirectToAction("ListLanguages");
                }
                else
                {
                    Session["LanguageMessage"] = "Создаеваемый язык уже добавлен в словарь.";
                    return RedirectToAction("ListLanguages");
                }
            }
            catch
            {
                Session["LanguageMessage"] = "Языка, с создаваемой аббревиатурой, не существует.";
                return RedirectToAction("ListLanguages");
            }
            
        }

        [HttpGet]
        [Route("EditLanguage/{id}")]
        public ActionResult EditLanguage(string id)
        {
            if (id == null)
            {
                Session["LanguageMessage"] = "Язык не найден.";
                return RedirectToAction("ListLanguages");
            }
            Language language = dbContext.Languages.Find(id);
            if(language != null)
            {
                return View(language);
            }
            Session["LanguageMessage"] = "Произошла неизвестная ошибка.";
            return RedirectToAction("ListLanguages");
        }

        [HttpPost]
        [Route("EditLanguage")]
        public ActionResult EditLanguage(Language language)
        {
            dbContext.Entry(language).State = EntityState.Modified;
            dbContext.SaveChanges();
            Session["LanguageMessage"] = "Язык успешно изменен.";
            return RedirectToAction("ListLanguages");
        }

        [HttpGet]
        [Route("DeleteLanguage/{id}")]
        public ActionResult DeleteLanguage(string id)
        {
            if (id == null)
            {
                Session["LanguageMessage"] = "Язык не найден.";
                return RedirectToAction("ListLanguages");
            }
            Language language = dbContext.Languages.Find(id);
            if (language != null)
            {
                return View(language);
            }
            Session["LanguageMessage"] = "Произошла неизвестная ошибка.";
            return RedirectToAction("ListLanguages");
        }

        [HttpPost, ActionName("DeleteLanguage")]
        public ActionResult DeleteLanguageConfirmed(string id)
        {
            Language language = dbContext.Languages.Find(id);
            if (language == null)
            {
                Session["LanguageMessage"] = "Язык не найден.";
                return RedirectToAction("ListLanguages");
            }
            dbContext.Languages.Remove(language);
            var deleteTranslations = dbContext.Translations.Where(t => t.LanguageId == id);
            foreach (Translation translation in deleteTranslations)
            {
                dbContext.Translations.Remove(translation);
            }
            dbContext.SaveChanges();
            Session["LanguageMessage"] = "Язык успешно удален.";
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
        [Route("CreateTemplate")]
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
            //добавление новых слов из template в язык
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
                    translation.LanguageNativeName = dbContext.Languages.Find(id).LanguageNativeName;
                    dbContext.Translations.Add(translation);
                }
            }
            dbContext.SaveChanges();
            foreach (var translation in translationsId)
            {
                if (!templatesId.Contains(translation))
                {
                    Translation deleteTranslation = dbContext.Translations.Find(id,translation);
                    dbContext.Entry(deleteTranslation).State = EntityState.Deleted;
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

        public ActionResult TranslateWord(string search)
        {
            if (search != "")
            {
                var activeLanguages = dbContext.Languages.Where(t => t.Activity == true).Select(t => t.LanguageId).ToList();
                var allTranslations = dbContext.Translations.Where(t => t.MessageId == search).Select(t => t);
                var translations = new List<Translation>();
                foreach (Translation translation in allTranslations)
                {
                    if (activeLanguages.Contains(translation.LanguageId))
                    {
                        translations.Add(translation);
                    }
                }
                if (translations.Count == 0)
                {
                    return Json(new { searchResult = "noTranslations", result = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { translationWords = RenderViewToString("TranslateWords", translations.ToList()) , result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { searchResult = "noSearch", result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        protected string RenderViewToString(string viewName, object model)
        {
            ControllerContext context = this.ControllerContext;
            if (string.IsNullOrEmpty(viewName))
                viewName = context.RouteData.GetRequiredString("action");

            var viewData = new ViewDataDictionary(model);

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}