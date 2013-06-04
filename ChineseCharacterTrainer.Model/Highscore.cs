using System;
using System.Runtime.Serialization;

namespace ChineseCharacterTrainer.Model
{
    [DataContract(IsReference = true)]
    public class Highscore : Entity
    {
        public Highscore(string username, Dictionary dictionary, QuestionResult questionResult)
        {
            Username = username;
            Dictionary = dictionary;
            QuestionResult = questionResult;
            DictionaryId = dictionary.Id;
        }

        protected Highscore() { }

        [DataMember]
        public string Username { get; private set; }

        [DataMember]
        public Guid DictionaryId { get; private set; }

        public virtual Dictionary Dictionary { get; private set; }

        [DataMember]
        public virtual QuestionResult QuestionResult { get; private set; }
    }
}
