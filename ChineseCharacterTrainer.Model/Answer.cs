using System;
using System.Runtime.Serialization;

namespace ChineseCharacterTrainer.Model
{
    [DataContract]
    public class Answer : Entity
    {
        public Answer(bool isCorrect, DateTime answerTime, TimeSpan duration, DictionaryEntry dictionaryEntry)
        {
            IsCorrect = isCorrect;
            AnswerTime = answerTime;
            Duration = duration;
            DictionaryEntry = dictionaryEntry;
            if (dictionaryEntry != null) DictionaryEntryId = dictionaryEntry.Id;
        }

        protected Answer() { }

        public virtual DictionaryEntry DictionaryEntry { get; private set; }

        [DataMember]
        public bool IsCorrect { get; private set; }

        [DataMember]
        public TimeSpan Duration { get; private set; }

        [DataMember]
        public DateTime AnswerTime { get; private set; }

        [DataMember]
        public Guid DictionaryEntryId { get; private set; }

        [DataMember]
        public Guid QuestionResultId { get; set; }

        public virtual QuestionResult QuestionResult { get; set; }
    }
}