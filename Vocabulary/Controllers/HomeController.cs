using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vocabulary.Logic;
using Vocabulary.Models;

namespace Vocabulary.Controllers
{
    public class HomeController : Controller
    {
        VocabularyContext dbContext = new VocabularyContext();
        VocabularyLogic vbLogic = new VocabularyLogic();
        private static int countTopWords = 5;

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
            dbContext.SaveChanges();
            vbLogic.deleteTranslationLanguage(id);
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
            var vocabularyTempalte = dbContext.Template.Find(template.TemplateMessage);
            if (vocabularyTempalte == null)
            {
                template.TemplateId = template.TemplateMessage;
                dbContext.Template.Add(template);
                dbContext.SaveChanges();
                Session["TemplateMessage"] = "Слово успешно добавлено.";
                return RedirectToAction("ListTemplates");
            }
            else
            {
                Session["TemplateMessage"] = "Слово уже добавлено в словарь.";
                return RedirectToAction("ListTemplates");
            }
        }

        [HttpGet]
        [Route("EditTemplate/{id}")]
        public ActionResult EditTemplate(string id)
        {
            if (id == null)
            {
                Session["TemplateMessage"] = "Слово не найдено.";
                return RedirectToAction("ListTemplates");
            }
            Template template = dbContext.Template.Find(id);
            if (template != null)
            {
                return View(template);
            }
            Session["TemplateMessage"] = "Произошла неизвестная ошибка.";
            return RedirectToAction("ListTemplates");
        }

        [HttpPost]
        [Route("EditTemplate")]
        public ActionResult EditTemplate(Template template)
        {
            dbContext.Entry(template).State = EntityState.Modified;
            dbContext.SaveChanges();
            Session["TemplateMessage"] = "Слово успешно изменено.";
            return RedirectToAction("ListTemplates");
        }

        [HttpGet]
        [Route("DeleteTemplate/{id}")]
        public ActionResult DeleteTemplate(string id)
        {
            if (id == null)
            {
                Session["TemplateMessage"] = "Слово не найдено.";
                return RedirectToAction("ListTemplates");
            }
            Template template = dbContext.Template.Find(id);
            if (template != null)
            {
                return View(template);
            }
            Session["TemplateMessage"] = "Произошла неизвестная ошибка.";
            return RedirectToAction("ListTemplates");
        }

        [HttpPost, ActionName("DeleteTemplate")]
        public ActionResult DeleteTemplateConfirmed(string id)
        {
            Template template = dbContext.Template.Find(id);
            if (template == null)
            {
                Session["TemplateMessage"] = "Слово не найдено.";
                return RedirectToAction("ListTemplates");
            }
            dbContext.Template.Remove(template);
            dbContext.SaveChanges();
            vbLogic.deleteTranslationTemplate(id);
            Session["TemplateMessage"] = "Слово успешно удалено.";
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
            vbLogic.updateTranslation(id);
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
                var translations = vbLogic.getTranslations(search);
                if (translations.Count == 0)
                {
                    return Json(new { searchResult = "noTranslations", result = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var popularWords = vbLogic.getPopularWords(countTopWords);
                    return Json(new { translationWords = RenderViewToString("TranslateWords", translations.ToList()), popularWords = RenderViewToString("Templates", popularWords.ToList()), result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { searchResult = "noSearch", result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PopularWords()
        {
            return View();
        }

        public ActionResult GetTemplates()
        {
            vbLogic.updatePopularWords();
            var popularWords = vbLogic.getPopularWords(countTopWords);
            return PartialView("Templates", popularWords.ToList());
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