using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows.Input;
using ChineseCharacterTrainer.Implementation.Services;
using ChineseCharacterTrainer.Library;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.Implementation.ViewModels
{
    public sealed class HighscoreVM : ViewModel, IHighscoreVM
    {
        private readonly IRepository _repository;
        private readonly IScoreCalculator _scoreCalculator;
        private ICommand _continueCommand;

        public HighscoreVM(IRepository repository, IScoreCalculator scoreCalculator)
        {
            _repository = repository;
            _scoreCalculator = scoreCalculator;
        }

        public ICommand ContinueCommand
        {
            get { return _continueCommand ?? (_continueCommand = new RelayCommand(p => RaiseReturnToMenuRequested())); }
        }

        public List<dynamic> Highscores { get; private set; }
        public int CurrentHighscore { get; private set; }
        public int PersonalBest { get; private set; }
        public event Action ReturnToMenuRequested;

        public void Initialize(Highscore currentHighscore)
        {
            CurrentHighscore = _scoreCalculator.CalculateScore(currentHighscore.QuestionResult);

            var allHighscores = _repository.GetAll<Highscore>();
            var highscoresForDictionary = GetOrderedHighscoresForDictionary(allHighscores, currentHighscore.DictionaryId);
            Highscores = GetBestAllTimeHighscores(highscoresForDictionary);
            PersonalBest = GetPersonalBest(highscoresForDictionary, currentHighscore.Username);
        }

        private void RaiseReturnToMenuRequested()
        {
            var handler = ReturnToMenuRequested;
            if (handler != null) handler();
        }

        private int GetPersonalBest(IEnumerable<Highscore> highscores, string username)
        {
            var personalBest = highscores.First(p => p.Username == username);
            return _scoreCalculator.CalculateScore(personalBest.QuestionResult);
        }

        private List<Highscore> GetOrderedHighscoresForDictionary(IEnumerable<Highscore> allHighscores, Guid dictionaryId)
        {
            return allHighscores
                .Where(p => p.DictionaryId == dictionaryId)
                .OrderBy(p => _scoreCalculator.CalculateScore(p.QuestionResult)).ToList();
        }

        private List<dynamic> GetBestAllTimeHighscores(IEnumerable<Highscore> highscores)
        {
            var i = 1;
            return highscores.Take(5).Select(
                p => CreateDynamicScoreObject(i++, p.Username, _scoreCalculator.CalculateScore(p.QuestionResult)))
                             .ToList();
        }

        private static dynamic CreateDynamicScoreObject(int rank, string username, int score)
        {
            dynamic expandoObject = new ExpandoObject();
            expandoObject.Ranking = rank;
            expandoObject.Username = username;
            expandoObject.Score = score;
            return expandoObject;
        }
    }
}