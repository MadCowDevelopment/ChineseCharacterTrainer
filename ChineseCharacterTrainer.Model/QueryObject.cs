using System;
using System.Runtime.Serialization;

namespace ChineseCharacterTrainer.Model
{
    [DataContract]
    public class QueryObject
    {
        public QueryObject(Guid dictionaryId, int numberOfEntries)
        {
            DictionaryId = dictionaryId;
            NumberOfEntries = numberOfEntries;
        }

        [DataMember]
        public Guid DictionaryId { get; private set; }

        [DataMember]
        public int NumberOfEntries { get; private set; }
    }
}
