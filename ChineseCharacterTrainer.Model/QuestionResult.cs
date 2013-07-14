using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ChineseCharacterTrainer.Model
{
    [DataContract]
    public class QuestionResult : Entity
    {
        public QuestionResult()
        {
            Answers = new List<Answer>();
        }

        public virtual List<Answer> Answers { get; private set; }

        public int NumberOfCorrectAnswers { get { return Answers.Count(p => p.IsCorrect); } }

        public int NumberOfIncorrectAnswers { get { return Answers.Count(p => !p.IsCorrect); } }

        public TimeSpan Duration
        {
            get
            {
                var duration = new TimeSpan();
                return Answers.Aggregate(duration, (current, answer) => current + answer.Duration);
            }
        }

        public void AddAnswer(Answer answer)
        {
            answer.QuestionResultId = Id;
            answer.QuestionResult = this;
            Answers.Add(answer);
        }
    }
}
