using System;
using System.Windows.Input;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.Implementation.ViewModels
{
    public interface IPracticeSummaryVM : ISummaryVM
    {
        ICommand ReturnToMenuCommand { get; }
        event Action ReturnToMenuRequested;
        void Initialize(QuestionResult questionResult);
    }
}