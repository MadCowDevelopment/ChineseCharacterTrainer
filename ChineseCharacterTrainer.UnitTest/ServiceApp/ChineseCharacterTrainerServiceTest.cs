using System;
using System.Collections.Generic;
using System.Linq;
using ChineseCharacterTrainer.Model;
using ChineseCharacterTrainer.ServiceApp;
using ChineseCharacterTrainer.ServiceApp.Persistence;
using Moq;
using NUnit.Framework;

namespace ChineseCharacterTrainer.UnitTest.ServiceApp
{
    class ChineseCharacterTrainerServiceTest
    {
        private ChineseCharacterTrainerService _objectUnderTest;
        private Mock<IChineseTrainerContext> _chineseTrainerContextMock;

        [SetUp]
        public void Initialize()
        {
            _chineseTrainerContextMock = new Mock<IChineseTrainerContext>();
            _objectUnderTest = new ChineseCharacterTrainerService();
            _objectUnderTest.ChineseTrainerContext = _chineseTrainerContextMock.Object;
        }

        [Test]
        public void ShouldAddDictionaryToContextWhenAdding()
        {
            var dictionary = CreateTestDictionaryWithEntries();

            _objectUnderTest.AddDictionary(dictionary);

            _chineseTrainerContextMock.Verify(p => p.Add(dictionary));
        }

        [Test]
        public void ShouldSaveChangesWhenAddingDictionary()
        {
            var dictionary = CreateTestDictionaryWithEntries();

            _objectUnderTest.AddDictionary(dictionary);

            _chineseTrainerContextMock.Verify(p => p.SaveChanges());
        }

        [Test]
        public void ShouldAddDictionaryEntryToContextWhenAdding()
        {
            var dictionary = CreateTestDictionaryWithEntries();

            _objectUnderTest.AddDictionaryEntry(dictionary.Entries[0]);

            _chineseTrainerContextMock.Verify(p => p.Add(dictionary.Entries[0]));
        }

        [Test]
        public void ShouldSaveChangesWhenAddingDictionaryEntry()
        {
            var dictionary = CreateTestDictionaryWithEntries();

            _objectUnderTest.AddDictionaryEntry(dictionary.Entries[0]);

            _chineseTrainerContextMock.Verify(p => p.SaveChanges());
        }

        [Test]
        public void ShouldAddHighscoreToContextWhenAdding()
        {
            var highscore = new Highscore("Frank", Guid.NewGuid(), null);

            _objectUnderTest.AddHighscore(highscore);

            _chineseTrainerContextMock.Verify(p => p.Add(highscore));
        }

        [Test]
        public void ShouldSaveChangesWhenAddingHighscore()
        {
            var highscore = new Highscore("Frank", Guid.NewGuid(), null);

            _objectUnderTest.AddHighscore(highscore);

            _chineseTrainerContextMock.Verify(p => p.SaveChanges());
        }

        [Test]
        public void ShouldReturnDictionariesFromContext()
        {
            _chineseTrainerContextMock.Setup(p => p.GetAll<Dictionary>()).Returns(
                new List<Dictionary> { CreateTestDictionaryWithEntries() }.AsQueryable());

            var dictionaries = _objectUnderTest.GetDictionaries();

            Assert.AreEqual(1, dictionaries.Count);
        }

        [Test]
        public void ShouldReturnDictionariyEntriesFromContext()
        {
            var dictionary = CreateTestDictionaryWithEntries();
            _chineseTrainerContextMock.Setup(p => p.GetAll<DictionaryEntry>()).Returns(
                new List<DictionaryEntry> { dictionary.Entries[0] }.AsQueryable());

            var dictionaryEntries = _objectUnderTest.GetDictionaryEntriesForDictionary(dictionary.Id);

            Assert.AreEqual(1, dictionaryEntries.Count);
        }

        [Test]
        public void ShouldReturnHighscoresFromContext()
        {
            var dictionary = CreateTestDictionaryWithEntries();

            _chineseTrainerContextMock.Setup(p => p.GetAll<Highscore>()).Returns(
                new List<Highscore> { new Highscore("Frank", dictionary.Id, null)}.AsQueryable);

            var highscores = _objectUnderTest.GetHighscoresForDictionary(dictionary.Id);

            Assert.AreEqual(1, highscores.Count);
        }

        [Test]
        public void ShouldReturnDefaultIfContextIsNotInitialize()
        {
            _objectUnderTest = new ChineseCharacterTrainerService();
            var context = _objectUnderTest.ChineseTrainerContext;

            Assert.IsInstanceOf<ChineseTrainerContext>(context);
        }

        private Dictionary CreateTestDictionaryWithEntries()
        {
            var entries = new List<DictionaryEntry> { new DictionaryEntry("你", "ni3", null) };
            var dictionary = new Dictionary("Test", entries);
            return dictionary;
        }
    }
}
