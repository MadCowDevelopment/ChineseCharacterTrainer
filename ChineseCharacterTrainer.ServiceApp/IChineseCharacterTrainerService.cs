using System;
using ChineseCharacterTrainer.Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace ChineseCharacterTrainer.ServiceApp
{
    [ServiceContract]
    public interface IChineseCharacterTrainerService
    {
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
        [ApplyDataContractResolver]
        List<DictionaryEntry> GetDictionaryEntriesForQueryObject(QueryObject queryObject);

        [OperationContract]
        [ApplyDataContractResolver]
        QuestionResult GetQuestionResultById(Guid questionResultId);
            
        [OperationContract]
        void AddDictionary(Dictionary dictionary);

        [OperationContract]
        void AddDictionaryEntry(DictionaryEntry dictionaryEntry);

        [OperationContract]
        void AddHighscore(Highscore highscore);

        [OperationContract]
        void AddQuestionResult(QuestionResult questionResult);

        [OperationContract]
        void AddAnswer(Answer answer);
    }
}
