using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChineseCharacterTrainer.Model
{
    [DataContract(IsReference = true)]
    public class QuestionResult : Entity
    {
        public QuestionResult()
        {
            Answers = new List<Answer>();
        }

        public virtual List<Answer> Answers { get; private set; }

        [DataMember]
        public int NumberOfCorrectAnswers { get; private set; }

        [DataMember]
        public int NumberOfIncorrectAnswers { get; private set; }

        [DataMember]
        public TimeSpan Duration { get; private set; }

        public void AddAnswer(Answer answer)
        {
            answer.QuestionResultId = Id;
            answer.QuestionResult = this;
            Answers.Add(answer);

            if (answer.IsCorrect) NumberOfCorrectAnswers++;
            else NumberOfIncorrectAnswers++;

            Duration += answer.Duration;
        }
    }
}
