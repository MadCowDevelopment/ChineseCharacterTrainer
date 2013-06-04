using System;
using ChineseCharacterTrainer.Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace ChineseCharacterTrainer.ServiceApp
{
    [ServiceContract]
    public interface IChineseCharacterTrainerService
    {
        //[OperationContract]
        //[ApplyDataContractResolver]
        //List<Entity> GetAll(string typeName);

        //[OperationContract]
        //void Add(string typeName, Entity entity);

        [OperationContract]
        [ApplyDataContractResolver]
        List<Dictionary> GetDictionaries();

        [OperationContract]
        [ApplyDataContractResolver]
        List<DictionaryEntry> GetDictionaryEntriesForDictionary(Guid dictionaryId);
        
        [OperationContract]
        [ApplyDataContractResolver]
        List<Highscore> GetHighscoresForDictionary(Guid dictionaryId);

        [OperationContract]
        void AddDictionary(Dictionary dictionary);

        [OperationContract]
        void AddDictionaryEntry(DictionaryEntry dictionaryEntry);

        [OperationContract]
        void AddHighscore(Highscore highscore);
    }
}
