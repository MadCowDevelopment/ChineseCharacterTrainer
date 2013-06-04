using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.Implementation.Services
{
    public interface IScoreCalculator
    {
        int CalculateScore(QuestionResult questionResult);
    }
}