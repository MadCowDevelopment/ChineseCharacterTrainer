using System;
using System.Runtime.Serialization;

namespace ChineseCharacterTrainer.Model
{
    [DataContract]
    public class QueryObject
    {
        public QueryObject(Guid dictionaryId)
        {
            DictionaryId = dictionaryId;

            NumberOfEntries = 16;
            InitialRatio = 0.81;
            TimeFactor = 0.2;
        }

        [DataMember]
        public Guid DictionaryId { get; private set; }

        [DataMember]
        public int NumberOfEntries { get; set; }

        [DataMember]
        public double InitialRatio { get; set; }

        [DataMember]
        public double TimeFactor { get; set; }
    }
}
