using System.Linq;
using ChineseCharacterTrainer.Model;
using ChineseCharacterTrainer.ServiceApp.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ChineseCharacterTrainer.ServiceApp
{
    public class ChineseCharacterTrainerService : IChineseCharacterTrainerService
    {
        private IChineseTrainerContext _chineseTrainerContext;
        private IEntriesSelector _entriesSelector;

        internal IChineseTrainerContext ChineseTrainerContext
        {
            get
            {
                return _chineseTrainerContext ??
                               (_chineseTrainerContext = new ChineseTrainerContext());
            }

            set { _chineseTrainerContext = value; }
        }

        internal IEntriesSelector EntriesSelector
        {
            get
            {
                return _entriesSelector ??
                (_entriesSelector = new SmartEntriesSelector());
            }

            set { _entriesSelector = value; }
        }

        public ChineseCharacterTrainerService()
        {
            Database.SetInitializer(new DontDropExistingDbCreateTablesIfModelChanged<ChineseTrainerContext>());
        }

        public List<Dictionary> GetDictionaries()
        {
            var result = ChineseTrainerContext.GetAll<Dictionary>();
            return result.ToList();
        }

        public List<DictionaryEntry> GetDictionaryEntriesForDictionary(Guid dictionaryId)
        {
            var result = ChineseTrainerContext.GetAll<DictionaryEntry>().Where(p => p.DictionaryId == dictionaryId);
            return result.ToList();
        }

        public List<Highscore> GetHighscoresForDictionary(Guid dictionaryId)
        {
            var result = ChineseTrainerContext.GetAll<Highscore>().Where(p => p.DictionaryId == dictionaryId).ToList();
            return result;
        }

        public List<DictionaryEntry> GetDictionaryEntriesForQueryObject(QueryObject queryObject)
        {
            var allEntries = GetDictionaryEntriesForDictionary(queryObject.DictionaryId);
            var selectedEntries = EntriesSelector.SelectEntries(allEntries, queryObject);
            return selectedEntries;
        }

        public QuestionResult GetQuestionResultById(Guid questionResultId)
        {
            var questionResult = ChineseTrainerContext.GetById<QuestionResult>(questionResultId);
            return questionResult;
        }

        public void AddDictionary(Dictionary dictionary)
        {
            ChineseTrainerContext.Add(dictionary);
            ChineseTrainerContext.SaveChanges();
        }

        public void AddDictionaryEntry(DictionaryEntry dictionaryEntry)
        {
            ChineseTrainerContext.Add(dictionaryEntry);
            ChineseTrainerContext.SaveChanges();
        }

        public void AddHighscore(Highscore highscore)
        {
            ChineseTrainerContext.Add(highscore);
            ChineseTrainerContext.SaveChanges();
        }

        public void AddQuestionResult(QuestionResult questionResult)
        {
            ChineseTrainerContext.Add(questionResult);
            ChineseTrainerContext.SaveChanges();
        }

        public void AddAnswer(Answer answer)
        {
            ChineseTrainerContext.Add(answer);
            ChineseTrainerContext.SaveChanges();
        }
    }
}
