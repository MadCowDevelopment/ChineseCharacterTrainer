using System;
using System.Runtime.Serialization;

namespace ChineseCharacterTrainer.Model
{
    [DataContract]
    public class QuestionResult : Entity
    {
        public QuestionResult(int numberOfCorrectAnswers, int numberOfIncorrectAnswers, TimeSpan duration)
        {
            NumberOfCorrectAnswers = numberOfCorrectAnswers;
            NumberOfIncorrectAnswers = numberOfIncorrectAnswers;
            Duration = duration;
        }

        protected QuestionResult() { }

        [DataMember]
        public int NumberOfCorrectAnswers { get; private set; }

        [DataMember]
        public int NumberOfIncorrectAnswers { get; private set; }

        [DataMember]
        public TimeSpan Duration { get; private set; }
    }
}
