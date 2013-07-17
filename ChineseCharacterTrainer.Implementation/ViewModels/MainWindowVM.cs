using System.Collections.Generic;
using ChineseCharacterTrainer.Library;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.Implementation.ViewModels
{
    public class MainWindowVM : ViewModel, IMainWindowVM
    {
        private readonly IMenuVM _menuVM;
        private IViewModel _content;
        private readonly IQuestionVM _questionVM;
        private readonly ICompetitionSummaryVM _competitionSummaryVM;
        private readonly IPracticeSummaryVM _practiceSummaryVM;
        private readonly IHighscoreVM _highscoreVM;

        public MainWindowVM(
            IMenuVM menuVM,
            IQuestionVM questionVM,
            ICompetitionSummaryVM competitionSummaryVM,
            IPracticeSummaryVM practiceSummaryVM,
            IHighscoreVM highscoreVM)
        {
            _questionVM = questionVM;
            _competitionSummaryVM = competitionSummaryVM;
            _practiceSummaryVM = practiceSummaryVM;
            _highscoreVM = highscoreVM;
            _menuVM = menuVM;

            _questionVM.QuestionsFinished += QuestionVMQuestionsFinished;
            _menuVM.StartCompetitionRequested += MenuVMStartCompetitionRequested;
            _menuVM.StartPracticeRequested += MenuVMStartPracticeRequested;
            _competitionSummaryVM.UploadFinished += SummaryVMUploadFinished;
            _practiceSummaryVM.ReturnToMenuRequested += PracticeSummaryVMReturnToMenuRequested;
            _highscoreVM.ReturnToMenuRequested += HighscoreVMReturnToMenuRequested;

            Content = _menuVM;
        }

        public IViewModel Content
        {
            get { return _content; }
            set { _content = value; RaisePropertyChanged(() => Content); }
        }

        public void Initialize()
        {
            _menuVM.Initialize();
        }

        private void HighscoreVMReturnToMenuRequested()
        {
            Content = _menuVM;
        }

        private void PracticeSummaryVMReturnToMenuRequested()
        {
            Content = _menuVM;
        }

        private void SummaryVMUploadFinished(Highscore highscore)
        {
            _highscoreVM.Initialize(highscore);
            Content = _highscoreVM;
        }

        private void MenuVMStartCompetitionRequested(Dictionary dictionary)
        {
            _trainingMode = TrainingMode.Competition;
            _questionVM.Initialize(dictionary.Entries);
            Content = _questionVM;
        }

        private TrainingMode _trainingMode;

        private void MenuVMStartPracticeRequested(List<DictionaryEntry> dictionaryEntries)
        {
            _trainingMode = TrainingMode.Practice;
            _questionVM.Initialize(dictionaryEntries);
            Content = _questionVM;
        }

        private void QuestionVMQuestionsFinished(QuestionResult questionResult)
        {
            switch (_trainingMode)
            {
                case TrainingMode.Competition:
                    _competitionSummaryVM.Initialize(_menuVM.SelectedDictionary, questionResult);
                    Content = _competitionSummaryVM;
                    break;
                case TrainingMode.Practice:
                    _practiceSummaryVM.Initialize(questionResult);
                    Content = _practiceSummaryVM;
                    break;
            }
        }

        private enum TrainingMode
        {
            Practice,
            Competition
        }
    }
}
