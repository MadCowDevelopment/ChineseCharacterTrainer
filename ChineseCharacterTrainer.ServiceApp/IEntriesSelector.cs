using System.Collections.Generic;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.ServiceApp
{
    public interface IEntriesSelector
    {
        List<DictionaryEntry> SelectEntries(List<DictionaryEntry> entries, QueryObject queryObject);
    }
}