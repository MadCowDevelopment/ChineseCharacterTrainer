using System;
using ChineseCharacterTrainer.Model;
using System.Collections.Generic;

namespace ChineseCharacterTrainer.Implementation.Services
{
    public interface IRepository
    {
        void AddDictionary(Dictionary dictionary);
        void AddHighscore(Highscore highscore);
        List<Dictionary> GetAllDictionaries();
        List<Highscore> GetAllHighscores(Guid dictionaryId);
    }
}