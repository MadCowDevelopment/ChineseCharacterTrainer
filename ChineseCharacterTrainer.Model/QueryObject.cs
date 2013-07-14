using System.Runtime.Serialization;

namespace ChineseCharacterTrainer.Model
{
    [DataContract]
    public class QueryObject
    {
        public QueryObject(int numberOfEntries)
        {
            NumberOfEntries = numberOfEntries;
        }

        [DataMember]
        public int NumberOfEntries { get; private set; }
    }
}
