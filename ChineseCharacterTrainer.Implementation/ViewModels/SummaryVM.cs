using ChineseCharacterTrainer.Implementation.ServiceReference;
using ChineseCharacterTrainer.Implementation.Services;
using ChineseCharacterTrainer.Library;
using System;
using System.Windows.Input;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.Implementation.ViewModels
{
    public class SummaryVM : ViewModel, ISummaryVM
    {
        private readonly IRepository _repository;
        private readonly IScoreCalculator _scoreCalculator;
        private string _username;
        private ICommand _uploadScoreCommand;

        private QuestionResult _questionResult;
        private Dictionary _dictionary;

        public SummaryVM(IRepository repository, IScoreCalculator scoreCalculator)
        {
            _repository = repository;
            _scoreCalculator = scoreCalculator;
        }

        public int NumberOfCorrectAnswers
        {
            get { return _questionResult.NumberOfCorrectAnswers; }
        }

        public int NumberOfIncorrectAnswers
        {
            get { return _questionResult.NumberOfIncorrectAnswers; }
        }

        public TimeSpan Duration
        {
            get { return _questionResult.Duration; }
        }

        public int Score
        {
            get { return _scoreCalculator.CalculateScore(_questionResult); }
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
                                   var highscore = new Highscore(Username, _dictionary, _questionResult);
                                   _repository.Add(highscore);
                                   RaiseUploadFinished(highscore);
                               }, p => !string.IsNullOrWhiteSpace(Username)));
            }
        }

        public void Initialize(Dictionary dictionary, QuestionResult questionResult)
        {
            _dictionary = dictionary;
            _questionResult = questionResult;
        }

        public event Action<Highscore> UploadFinished;

        public void RaiseUploadFinished(Highscore highScore)
        {
            var handler = UploadFinished;
            if (handler != null) handler(highScore);
        }
    }
}
