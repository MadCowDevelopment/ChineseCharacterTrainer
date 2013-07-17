using ChineseCharacterTrainer.Implementation.Services;
using ChineseCharacterTrainer.Implementation.ViewModels;
using ChineseCharacterTrainer.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ChineseCharacterTrainer.UnitTest.ViewModels
{
    class HighscoreVMTest
    {
        private IHighscoreVM _objectUnderTest;

        private Mock<IRepository> _repositoryMock;

        private readonly Dictionary _testDictionary = new Dictionary("TestDict1", null);

        private Highscore _currentHighscore;

        [SetUp]
        public void Initialize()
        {
            _currentHighscore = CreateCurrentHighscore();

            var highscores = new List<Highscore>
                {
                    CreateHighscore("Frank", _testDictionary, 80),
                    CreateHighscore("Myself", _testDictionary, 50),
                    CreateHighscore("Sandra", _testDictionary, 40),
                    CreateHighscore("Sandra", _testDictionary, 70),
                    CreateHighscore("Myself", new Dictionary("dict2", null), 20),
                    _currentHighscore,
                    CreateHighscore("Frank", _testDictionary, 30),
                    CreateHighscore("Frank", _testDictionary, 60)
                };

            _repositoryMock = new Mock<IRepository>();
            _repositoryMock.Setup(p => p.GetAllHighscores(_testDictionary.Id)).Returns(highscores);

            _objectUnderTest = new HighscoreVM(_repositoryMock.Object, new ScoreCalculator());
        }

        [Test]
        public void ShouldRaiseEventWhenContinueCommandIsExecuted()
        {
            var eventWasRaised = false;
            _objectUnderTest.ReturnToMenuRequested += () => eventWasRaised = true;

            _objectUnderTest.ReturnToMenuCommand.Execute(null);

            Assert.IsTrue(eventWasRaised);
        }

        [Test]
        public void ShouldSetCurrentHighscoreWhenInitializing()
        {
            _objectUnderTest.Initialize(_currentHighscore);

            Assert.AreEqual(10, _objectUnderTest.CurrentHighscore);
        }

        [Test]
        public void ShouldGetBestHighscoreWhenInitializing()
        {
            _objectUnderTest.Initialize(_currentHighscore);

            Assert.AreEqual(5, _objectUnderTest.Highscores.Count);
            Assert.AreEqual(10, _objectUnderTest.Highscores[0].Score);
            Assert.AreEqual(30, _objectUnderTest.Highscores[1].Score);
            Assert.AreEqual(40, _objectUnderTest.Highscores[2].Score);
            Assert.AreEqual(50, _objectUnderTest.Highscores[3].Score);
            Assert.AreEqual(60, _objectUnderTest.Highscores[4].Score);
        }

        [Test]
        public void ShouldGetBestUserHighscoreWhenInitializing()
        {
            _objectUnderTest.Initialize(_currentHighscore);

            Assert.AreEqual(10, _objectUnderTest.PersonalBest);
        }

        private Highscore CreateCurrentHighscore()
        {
            return CreateHighscore("Myself", _testDictionary, 10);
        }

        private static Highscore CreateHighscore(string username, Dictionary dictionary, int seconds)
        {
            var questionResult = new QuestionResult();
            questionResult.AddAnswer(new Answer(true, DateTime.Now, TimeSpan.FromSeconds(seconds), null));
            return new Highscore(username, dictionary.Id, questionResult);
        }
    }
}
