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

        [TestCase(10, 1, 0, 10)]
        [TestCase(10, 0, 2, 20)]
        [TestCase(100, 1, 0, 100)]
        public void ShouldReturnCorrectScore(double seconds, int numberOfCorrectAnswers, int numberOfIncorrectAnswers, int expectedScore)
        {
            var questionResult = new QuestionResult();

            for (var i = 0; i < numberOfCorrectAnswers; i++)
            {
                questionResult.AddAnswer(new Answer(true,
                                                    DateTime.Now,
                                                    TimeSpan.FromSeconds(seconds/
                                                                         (numberOfCorrectAnswers +
                                                                          numberOfIncorrectAnswers)), null));
            }

            for (var i = 0; i < numberOfIncorrectAnswers; i++)
            {
                questionResult.AddAnswer(new Answer(false,
                                                    DateTime.Now,
                                                    TimeSpan.FromSeconds(seconds/
                                                                         (numberOfCorrectAnswers +
                                                                          numberOfIncorrectAnswers)), null));
            }

            var score = _objectUnderTest.CalculateScore(questionResult);

            Assert.AreEqual(expectedScore, score);
        }
    }
}
