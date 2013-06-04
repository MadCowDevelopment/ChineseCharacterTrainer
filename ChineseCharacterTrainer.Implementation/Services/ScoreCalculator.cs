using System;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.Implementation.Services
{
    public class ScoreCalculator : IScoreCalculator
    {
        public int CalculateScore(QuestionResult questionResult)
        {
            return (int)questionResult.Duration.TotalSeconds + questionResult.NumberOfIncorrectAnswers * 5;
        }
    }
}