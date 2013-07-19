using ChineseCharacterTrainer.Model;
using ChineseCharacterTrainer.ServiceApp;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ChineseCharacterTrainer.UnitTest.ServiceApp
{
    public class SmartEntriesSelectorTest
    {
        private SmartEntriesSelector _objectUnderTest;
        private QueryObject _queryObject;
        private List<DictionaryEntry> _entries;
        private DictionaryEntry _entry1;
        private DictionaryEntry _entry2;

        [SetUp]
        public void Initialize()
        {
            _queryObject = new QueryObject(new Guid(), 1);
            _entries = new List<DictionaryEntry>();
            CreateEntries();

            _objectUnderTest = new SmartEntriesSelector();
        }

        [Test]
        public void ShouldSelectEntryWithWorseRatioWhenLastAnswerTimeIsEqual()
        {
            AddAnswer(_entry1, false, new DateTime(2010, 1, 1));
            AddAnswer(_entry2, true, new DateTime(2010, 1, 1));

            AssertCorrectItemGetsSelected(_entry1);
        }

        [Test]
        public void ShouldSelectEntryWithAnswerTimeThatIsLongerAgoWhenRatioIsEqual()
        {
            AddAnswer(_entry1, true, new DateTime(2010, 1, 1));
            AddAnswer(_entry2, true, new DateTime(2010, 1, 2));

            AssertCorrectItemGetsSelected(_entry1);
        }

        [Test]
        public void ShouldSelectEntryWithNoAnswer()
        {
            AddAnswer(_entry2, false, new DateTime(2010, 1, 2));

            AssertCorrectItemGetsSelected(_entry1);
        }

        [Test]
        public void ShouldGetOneEntryIfThereAreNoAnswersAtAll()
        {
            AssertCorrectItemGetsSelected(_entry1);
        }

        [Test]
        public void ShouldReturnEmptyListWhenPassedInEmptyList()
        {
            var entries = _objectUnderTest.SelectEntries(new List<DictionaryEntry>(), new QueryObject(Guid.NewGuid(), 1));

            Assert.AreEqual(0, entries.Count);
        }

        private static void AddAnswer(DictionaryEntry entry, bool isCorrect, DateTime answerTime)
        {
            entry.Answers.Add(new Answer(isCorrect, answerTime, TimeSpan.FromSeconds(1), entry));
        }

        private void AssertCorrectItemGetsSelected(DictionaryEntry entry1)
        {
            var selectedEntries = SelectEntries();
            Assert.AreEqual(1, selectedEntries.Count);
            Assert.AreEqual(entry1, selectedEntries[0]);
        }

        private List<DictionaryEntry> SelectEntries()
        {
            var selectedEntries = _objectUnderTest.SelectEntries(_entries, _queryObject);
            return selectedEntries;
        }

        private void CreateEntries()
        {
            _entry1 = new DictionaryEntry("你", "ni3", new List<Translation> {new Translation("you")});
            _entries.Add(_entry1);

            _entry2 = new DictionaryEntry("我", "wo3", new List<Translation> { new Translation("I") });
            _entries.Add(_entry2);
        }
    }
}
