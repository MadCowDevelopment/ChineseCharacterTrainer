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

        public IChineseTrainerContext ChineseTrainerContext
        {
            get
            {
                return _chineseTrainerContext ??
                       (_chineseTrainerContext = new ChineseTrainerContext());
            }

            set { _chineseTrainerContext = value; }
        }

        public ChineseCharacterTrainerService()
        {
            Database.SetInitializer(new DontDropExistingDbCreateTablesIfModelChanged<ChineseTrainerContext>());
        }

        //public List<Entity> GetAll(string typeName)
        //{
        //    var result = ChineseTrainerContext.GetAll(Type.GetType(typeName));
        //    return result;
        //}

        //public void Add(string typeName, Entity entity)
        //{
        //    ChineseTrainerContext.Add(Type.GetType(typeName), entity);
        //    ChineseTrainerContext.SaveChanges();
        //}

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
    }
}
