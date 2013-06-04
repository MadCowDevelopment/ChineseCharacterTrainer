using System;
using System.Runtime.Serialization;

namespace ChineseCharacterTrainer.Model
{
    [DataContract(IsReference = true)]
    public class Highscore : Entity
    {
        public Highscore(string username, Guid dictionaryId, QuestionResult questionResult)
        {
            Username = username;
            QuestionResult = questionResult;
            DictionaryId = dictionaryId;
        }

        protected Highscore() { }

        [DataMember]
        public string Username { get; private set; }

        [DataMember]
        public Guid DictionaryId { get; private set; }

        [DataMember]
        public virtual QuestionResult QuestionResult { get; private set; }
    }
}
