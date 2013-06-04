using ChineseCharacterTrainer.Implementation.Services;
using ChineseCharacterTrainer.Model;
using NUnit.Framework;
using System;

namespace ChineseCharacterTrainer.UnitTest.Services
{
    class ScoreCalculatorTest
    {
        private IScoreCalculator _objectUnderTest;

        [SetUp]
        public void Initialize()
        {
            _objectUnderTest = new ScoreCalculator();
        }

        [TestCase(10, 0, 10)]
        [TestCase(10, 2, 20)]
        [TestCase(100, 0, 100)]
        public void ShouldReturnCorrectScore(double seconds, int numberOfIncorrectAnswers, int expectedScore)
        {
            var score =
                _objectUnderTest.CalculateScore(
                    new QuestionResult(0, numberOfIncorrectAnswers, TimeSpan.FromSeconds(seconds)));

            Assert.AreEqual(expectedScore, score);
        }
    }
}
