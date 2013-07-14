using System;
using System.Linq;
using ChineseCharacterTrainer.Model;
using ChineseCharacterTrainer.ServiceApp.Persistence;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;

namespace ChineseCharacterTrainer.IntegrationTest
{
    public class ChineseTrainerContextTest
    {
        private const string TestDatabaseName = "ChineseCharacterTrainerTest";

        [TestFixtureSetUp]
        public void Initialize()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<ChineseTrainerContext>());

            var testSetupContext = new ChineseTrainerContext(TestDatabaseName);

            Guard<Dictionary>(testSetupContext);
            Guard<DictionaryEntry>(testSetupContext);
            Guard<Translation>(testSetupContext);
            Guard<Highscore>(testSetupContext);
            Guard<Answer>(testSetupContext);

            var dictionary1 = CreateDictionary("1");
            testSetupContext.Add(dictionary1);

            var dictionary2 = CreateDictionary("2");
            testSetupContext.Add(dictionary2);

            var highscore = CreateHighscore(dictionary1);
            testSetupContext.Add(highscore);

            testSetupContext.SaveChanges();
        }

        private static Highscore CreateHighscore(Dictionary dictionary1)
        {
            var questionResult = new QuestionResult();
            questionResult.AddAnswer(new Answer(true, DateTime.Now, TimeSpan.FromSeconds(1), dictionary1.Entries[0]));
            return new Highscore("Frank", dictionary1.Id, questionResult);
        }

        private static void Guard<T>(ChineseTrainerContext objectUnderTest) where T : class
        {
            Assert.AreEqual(0, objectUnderTest.GetAll<T>().Count(),
                            string.Format("Guard: Table for type {0} should be empty.", typeof(T)));
        }

        private Dictionary CreateDictionary(string name)
        {
            var entries = new List<DictionaryEntry>
                {
                    new DictionaryEntry("你", "ni3", new List<Translation> {new Translation("you")}),
                    new DictionaryEntry("走", "zou3", new List<Translation> {new Translation("go")})
                };

            var dictionary = new Dictionary(name, entries);
            return dictionary;
        }

        [Test]
        public void ShouldGetDictionariesFromDatabase()
        {
            var objectUnderTest = new ChineseTrainerContext(TestDatabaseName);
            var dictionaries = objectUnderTest.GetAll<Dictionary>().ToList();

            Assert.AreEqual(2, dictionaries.Count);
        }

        [Test]
        public void ShouldGetEntriesFromDatabase()
        {
            var objectUnderTest = new ChineseTrainerContext(TestDatabaseName);
            var entries = objectUnderTest.GetAll<DictionaryEntry>().ToList();

            Assert.AreEqual(4, entries.Count);
        }

        [Test]
        public void ShouldGetTranslationsFromDatabase()
        {
            var objectUnderTest = new ChineseTrainerContext(TestDatabaseName);
            var translations = objectUnderTest.GetAll<Translation>().ToList();

            Assert.AreEqual(4, translations.Count);
        }

        [Test]
        public void ShouldGetHighscoreFromDatabase()
        {
            var objectUnderTest = new ChineseTrainerContext(TestDatabaseName);

            var highscores = objectUnderTest.GetAll<Highscore>().ToList();

            Assert.AreEqual(1, highscores.Count);
        }

        [Test]
        public void ShouldGetQuestionResultsFromDatabase()
        {
            var objectUnderTest = new ChineseTrainerContext(TestDatabaseName);

            var questionResults = objectUnderTest.GetAll<QuestionResult>().ToList();

            Assert.AreEqual(1, questionResults.Count);
            Assert.AreEqual(1, questionResults[0].Answers.Count);
        }
    }
}
