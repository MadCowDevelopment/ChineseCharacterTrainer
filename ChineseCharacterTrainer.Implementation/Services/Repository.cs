using System;
using System.Linq;
using ChineseCharacterTrainer.Implementation.ServiceReference;
using ChineseCharacterTrainer.Model;
using System.Collections.Generic;

namespace ChineseCharacterTrainer.Implementation.Services
{
    public class Repository : IRepository
    {
        private readonly IChineseCharacterTrainerService _chineseCharacterTrainerService;

        public Repository(IChineseCharacterTrainerService chineseCharacterTrainerService)
        {
            _chineseCharacterTrainerService = chineseCharacterTrainerService;
        }

        //public List<T> GetAll<T>() where T : Entity
        //{
        //    var result = _chineseCharacterTrainerService.GetAll(typeof (T).AssemblyQualifiedName);
        //    return result.Select(p => p as T).ToList();
        //}

        //public void Add<T>(T entity) where T : Entity
        //{
        //    _chineseCharacterTrainerService.Add(typeof(T).AssemblyQualifiedName, entity);
        //}

        public void AddDictionary(Dictionary dictionary)
        {
            _chineseCharacterTrainerService.AddDictionary(dictionary);
            foreach (var entry in dictionary.Entries)
            {
                _chineseCharacterTrainerService.AddDictionaryEntry(entry);
            }
        }

        public void AddHighscore(Highscore highscore)
        {
            _chineseCharacterTrainerService.AddHighscore(highscore);
        }

        public List<Dictionary> GetAllDictionaries()
        {
            var dictionaries = _chineseCharacterTrainerService.GetDictionaries();
            foreach (var dictionary in dictionaries)
            {
                dictionary.Entries = _chineseCharacterTrainerService.GetDictionaryEntriesForDictionary(dictionary.Id);
            }

            return dictionaries;
        }

        public List<Highscore> GetAllHighscores(Guid dictionaryId)
        {
            var highscores = _chineseCharacterTrainerService.GetHighscoresForDictionary(dictionaryId);
            return highscores;
        }
    }
}