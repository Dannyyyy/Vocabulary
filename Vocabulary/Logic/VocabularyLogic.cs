using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Vocabulary.Models;

namespace Vocabulary.Logic
{
    public class VocabularyLogic
    {
        VocabularyContext dbContext = new VocabularyContext();

        public void deleteTranslationLanguage(string id)
        {
            var deleteTranslations = dbContext.Translations.Where(t => t.LanguageId == id);
            foreach (Translation translation in deleteTranslations)
            {
                dbContext.Translations.Remove(translation);
            }
            dbContext.SaveChanges();
        }

        public void deleteTranslationTemplate(string id)
        {
            var deleteTranslations = dbContext.Translations.Where(t => t.MessageId == id);
            foreach (Translation translation in deleteTranslations)
            {
                dbContext.Translations.Remove(translation);
            }
            dbContext.SaveChanges();
        }

        public void updateTranslation(string id)
        {
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
            //dbContext.SaveChanges();
            //удаление неиспользуемых слов 
            foreach (var translation in translationsId)
            {
                if (!templatesId.Contains(translation))
                {
                    Translation deleteTranslation = dbContext.Translations.Find(id, translation);
                    dbContext.Entry(deleteTranslation).State = EntityState.Deleted;
                }
            }
            dbContext.SaveChanges();
        }

        public void updatePopularWords()
        {
            //добавление новых слов из template в язык
            var templatesId = dbContext.Template.Select(t => t.TemplateId).ToList();
            var popularWordsId = dbContext.PopularWords.Select(t => t.WordId).ToList();
            foreach (var template in templatesId)
            {
                if (!popularWordsId.Contains(template))
                {
                    PopularWord popularWord = new PopularWord();
                    popularWord.WordId = template;
                    popularWord.Description = dbContext.Template.Find(template).Description;
                    popularWord.Count = 0;
                    dbContext.PopularWords.Add(popularWord);
                }
            }
            //dbContext.SaveChanges();
            //удаление неиспользуемых слов 
            foreach (var popularWord in popularWordsId)
            {
                if (!templatesId.Contains(popularWord))
                {
                    PopularWord deletePopularWord = dbContext.PopularWords.Find(popularWord);
                    dbContext.Entry(deletePopularWord).State = EntityState.Deleted;
                }
            }
            dbContext.SaveChanges();
        }

        public List<Translation> getTranslations(string search)
        {
            var activeLanguages = dbContext.Languages.Where(t => t.Activity == true).Select(t => t.LanguageId).ToList();
            var allTranslations = dbContext.Translations.Where(t => t.MessageId == search).Select(t => t);
            var translations = new List<Translation>();
            var popularWord = dbContext.PopularWords.Find(search);
            if (popularWord != null)
            {
                popularWord.Count++;
                dbContext.Entry(popularWord).State = EntityState.Modified;
                dbContext.SaveChanges();
            }
            foreach (Translation translation in allTranslations)
            {
                if (activeLanguages.Contains(translation.LanguageId))
                {
                    translations.Add(translation);
                }
            }
            return translations;
        }

        public IQueryable<PopularWord> getPopularWords(int count=5)
        {
            return dbContext.PopularWords.OrderByDescending(t => t.Count).Take(count);
        }
    }
}