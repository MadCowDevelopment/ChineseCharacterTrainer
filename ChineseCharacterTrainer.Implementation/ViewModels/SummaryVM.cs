using System;
using ChineseCharacterTrainer.Library;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.Implementation.ViewModels
{
    public abstract class SummaryVM : ViewModel, ISummaryVM
    {
        protected QuestionResult QuestionResult { get; private set; }

        public int NumberOfCorrectAnswers
        {
            get { return QuestionResult.NumberOfCorrectAnswers; }
        }

        public int NumberOfIncorrectAnswers
        {
            get { return QuestionResult.NumberOfIncorrectAnswers; }
        }

        public TimeSpan Duration
        {
            get { return QuestionResult.Duration; }
        }

        public void Initialize(QuestionResult questionResult)
        {
            QuestionResult = questionResult;
        }
    }
}
