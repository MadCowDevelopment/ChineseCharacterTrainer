using System;
using System.Windows.Input;
using ChineseCharacterTrainer.Implementation.Services;
using ChineseCharacterTrainer.Library;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.Implementation.ViewModels
{
    public class PracticeSummaryVM : SummaryVM, IPracticeSummaryVM
    {
        private readonly IRepository _repository;

        public PracticeSummaryVM(IRepository repository)
        {
            _repository = repository;
        }

        private ICommand _backToMenuCommand;

        public ICommand ReturnToMenuCommand
        {
            get
            {
                return _backToMenuCommand ?? (_backToMenuCommand = new RelayCommand(p =>
                    {
                        _repository.AddQuestionResult(QuestionResult);
                        RaiseReturnToMenuRequested();
                    }));
            }
        }

        public event Action ReturnToMenuRequested;

        private void RaiseReturnToMenuRequested()
        {
            var handler = ReturnToMenuRequested;
            if (handler != null) handler();
        }
    }

    public interface IPracticeSummaryVM : ISummaryVM
    {
        ICommand ReturnToMenuCommand { get; }
        event Action ReturnToMenuRequested;
        void Initialize(QuestionResult questionResult);
    }
}
