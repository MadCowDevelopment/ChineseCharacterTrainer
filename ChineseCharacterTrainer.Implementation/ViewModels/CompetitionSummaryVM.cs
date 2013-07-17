using System;
using System.Windows.Input;
using ChineseCharacterTrainer.Implementation.Services;
using ChineseCharacterTrainer.Library;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.Implementation.ViewModels
{
    public class CompetitionSummaryVM : SummaryVM, ICompetitionSummaryVM
    {
        private readonly IRepository _repository;
        private readonly IScoreCalculator _scoreCalculator;
        private string _username;
        private ICommand _uploadScoreCommand;
        private Dictionary _dictionary;

        public CompetitionSummaryVM(IRepository repository, IScoreCalculator scoreCalculator)
        {
            _repository = repository;
            _scoreCalculator = scoreCalculator;
        }

        
        public int Score
        {
            get { return _scoreCalculator.CalculateScore(QuestionResult); }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; RaisePropertyChanged(() => Username); }
        }

        public ICommand UploadScoreCommand
        {
            get
            {
                return _uploadScoreCommand ??
                       (_uploadScoreCommand =
                        new RelayCommand(
                            p =>
                                {
                                    var highscore = new Highscore(Username, _dictionary.Id, QuestionResult);
                                    _repository.AddHighscore(highscore);
                                    RaiseUploadFinished(highscore);
                                }, p => !string.IsNullOrWhiteSpace(Username)));
            }
        }

        public void Initialize(Dictionary dictionary, QuestionResult questionResult)
        {
            Initialize(questionResult);
            _dictionary = dictionary;
        }

        public event Action<Highscore> UploadFinished;

        private void RaiseUploadFinished(Highscore highScore)
        {
            var handler = UploadFinished;
            if (handler != null) handler(highScore);
        }
    }
}