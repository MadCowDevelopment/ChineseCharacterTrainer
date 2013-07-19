using ChineseCharacterTrainer.Implementation.ServiceReference;
using ChineseCharacterTrainer.Model;
using System;
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
            AddQuestionResult(highscore.QuestionResult);
            _chineseCharacterTrainerService.AddHighscore(highscore);
        }

        public void AddQuestionResult(QuestionResult questionResult)
        {
            _chineseCharacterTrainerService.AddQuestionResult(questionResult);
            foreach (var answer in questionResult.Answers)
            {
                _chineseCharacterTrainerService.AddAnswer(answer);
            }
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

        public List<DictionaryEntry> GetDictionaryEntriesForQueryObject(QueryObject queryObject)
        {
            return _chineseCharacterTrainerService.GetDictionaryEntriesForQueryObject(queryObject);
        }

        public List<Highscore> GetAllHighscores(Guid dictionaryId)
        {
            var highscores = _chineseCharacterTrainerService.GetHighscoresForDictionary(dictionaryId);
            foreach (var highscore in highscores)
            {
                highscore.QuestionResult =
                    _chineseCharacterTrainerService.GetQuestionResultById(highscore.QuestionResultId);
            }

            return highscores;
        }
    }
}