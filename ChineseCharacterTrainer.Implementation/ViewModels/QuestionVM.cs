using System;
using System.Collections.Generic;
using System.Windows.Input;
using ChineseCharacterTrainer.Implementation.Services;
using ChineseCharacterTrainer.Implementation.Utilities;
using ChineseCharacterTrainer.Library;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.Implementation.ViewModels
{
    public class QuestionVM : ViewModel, IQuestionVM
    {
        private readonly IDateTime _dateTime;
        private readonly IDictionaryEntryPicker _dictionaryEntryPicker;
        private ICommand _answerCommand;
        private DictionaryEntry _currentEntry;
        private string _answer;
        private bool _isInAnswerMode;
        private bool _lastAnswerWasCorrect;
        private DateTime _startTime;

        private QuestionResult _questionResult;

        public QuestionVM(
            IDateTime dateTime, 
            IDictionaryEntryPicker dictionaryEntryPicker)
        {
            _dateTime = dateTime;
            _dictionaryEntryPicker = dictionaryEntryPicker;
        }

        public event Action<QuestionResult> QuestionsFinished;

        public void Initialize(List<DictionaryEntry> dictionaryEntries)
        {
            _dictionaryEntryPicker.Initialize(dictionaryEntries);
            _questionResult = new QuestionResult();
            Answer = string.Empty;
            IsInAnswerMode = true;
            GetNextEntry();
        }

        public ICommand AnswerCommand
        {
            get { return _answerCommand ?? (_answerCommand = new RelayCommand(p => ProcessCurrentEntry())); }
        }

        public bool IsInAnswerMode
        {
            get { return _isInAnswerMode; }
            private set { _isInAnswerMode = value; RaisePropertyChanged(() => IsInAnswerMode);}
        }

        public bool LastAnswerWasCorrect
        {
            get { return _lastAnswerWasCorrect; }
            private set { _lastAnswerWasCorrect = value; RaisePropertyChanged(() => LastAnswerWasCorrect); }
        }

        public string Answer
        {
            get { return _answer; }
            set { _answer = value; RaisePropertyChanged(() => Answer); }
        }

        public int NumberOfCorrectAnswers
        {
            get { return _questionResult.NumberOfCorrectAnswers; }
        }

        public int NumberOfIncorrectAnswers
        {
            get { return _questionResult.NumberOfIncorrectAnswers; }
        }

        public DictionaryEntry CurrentEntry
        {
            get { return _currentEntry; }
            private set { _currentEntry = value; RaisePropertyChanged(() => CurrentEntry);}
        }

        private void RaiseQuestionsFinished(QuestionResult result)
        {
            var handler = QuestionsFinished;
            if (handler != null) handler(result);
        }

        private void ProcessCurrentEntry()
        {
            if (CurrentEntry == null)
            {
                return;
            }

            if (IsInAnswerMode)
            {
                AnswerCurrentEntry();
            }
            else
            {
                MoveToNextEntry();
            }
            
            IsInAnswerMode = !IsInAnswerMode;
        }

        private void MoveToNextEntry()
        {
            GetNextEntry();
            if (CurrentEntry == null)
            {
                RaiseQuestionsFinished(_questionResult);
                return;
            }

            Answer = string.Empty;
        }

        private void AnswerCurrentEntry()
        {
            LastAnswerWasCorrect = RemoveWhitespaces(Answer) == RemoveWhitespaces(CurrentEntry.Pinyin);

            if (LastAnswerWasCorrect)
            {
                AddAnswer(true);
            }
            else
            {
                AddAnswer(false);
                _dictionaryEntryPicker.QueueEntry(CurrentEntry);
            }
        }

        private void AddAnswer(bool isCorrect)
        {
            _questionResult.AddAnswer(new Answer(isCorrect, _dateTime.Now, _dateTime.Now - _startTime, CurrentEntry));
            RaisePropertyChanged(() => NumberOfCorrectAnswers);
            RaisePropertyChanged(() => NumberOfIncorrectAnswers);
        }

        private string RemoveWhitespaces(string value)
        {
            var result = value.Replace(" ", "");
            return result;
        }

        private void GetNextEntry()
        {
            _startTime = _dateTime.Now;
            CurrentEntry = _dictionaryEntryPicker.GetNextEntry();
        }
    }
}
