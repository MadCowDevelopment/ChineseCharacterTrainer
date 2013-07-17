using ChineseCharacterTrainer.Implementation.Services;
using ChineseCharacterTrainer.Implementation.ViewModels;
using ChineseCharacterTrainer.Model;
using Moq;
using NUnit.Framework;
using System;

namespace ChineseCharacterTrainer.UnitTest.ViewModels
{
    public class SummaryVMTest
    {
        private ICompetitionSummaryVM _objectUnderTest;

        private QuestionResult _questionResult;
        private readonly Dictionary _dictionary = new Dictionary("Test", null);

        private Mock<IRepository> _repositoryMock;
        private Mock<IScoreCalculator> _scoreCalculatorMock;

        [SetUp]
        public void Initialize()
        {
            _questionResult = new QuestionResult();
            _questionResult.AddAnswer(new Answer(true, DateTime.Now, TimeSpan.FromSeconds(1), null));
            _questionResult.AddAnswer(new Answer(false, DateTime.Now, TimeSpan.FromSeconds(1), null));
            _questionResult.AddAnswer(new Answer(false, DateTime.Now, TimeSpan.FromSeconds(1), null));

            _repositoryMock = new Mock<IRepository>();
            _scoreCalculatorMock = new Mock<IScoreCalculator>();

            _objectUnderTest = new CompetitionSummaryVM(_repositoryMock.Object, _scoreCalculatorMock.Object);
            _objectUnderTest.Initialize(_dictionary, _questionResult);
        }

        [Test]
        public void ShouldGetCorrectNumberOfCorrectAnswersAfterInitializing()
        {
            Assert.AreEqual(1, _objectUnderTest.NumberOfCorrectAnswers);
        }

        [Test]
        public void ShouldGetCorrectNumberOfIncorrectAnswersAfterInitializing()
        {
            Assert.AreEqual(2, _objectUnderTest.NumberOfIncorrectAnswers);
        }

        [Test]
        public void ShouldGetCorrectDurationAfterInitializing()
        {
            Assert.AreEqual(TimeSpan.FromSeconds(3), _objectUnderTest.Duration);
        }

        [Test]
        public void ShouldGetCorrectScoreAfterInitializing()
        {
            _scoreCalculatorMock.Setup(p => p.CalculateScore(_questionResult)).Returns(100);

            Assert.AreEqual(100, _objectUnderTest.Score);
        }

        [Test]
        public void CanUploadHighscoreWhenUsernameIsEntered()
        {
            _objectUnderTest.Username = "Frank";

            var canExecute = _objectUnderTest.UploadScoreCommand.CanExecute(null);

            Assert.IsTrue(canExecute);
        }

        [Test]
        public void CannotUploadHighscoreWhenUsernameIsEmpty()
        {
            _objectUnderTest.Username = null;

            var canExecute = _objectUnderTest.UploadScoreCommand.CanExecute(null);

            Assert.IsFalse(canExecute);
        }

        [Test]
        public void ShouldUploadHighscore()
        {
            _objectUnderTest.Username = "Frank";

            _objectUnderTest.UploadScoreCommand.Execute(null);

            _repositoryMock.Verify(p=>p.AddHighscore(It.IsAny<Highscore>()));
        }

        [Test]
        public void ShouldRaiseEventAfterUploadHighscore()
        {
            Highscore score = null;
            _objectUnderTest.UploadFinished += highscore => score = highscore;
            _objectUnderTest.Username = "Frank";
            
            _objectUnderTest.UploadScoreCommand.Execute(null);

            Assert.IsNotNull(score);
        }
    }
}
