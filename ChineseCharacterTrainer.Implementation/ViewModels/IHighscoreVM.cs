using System;
using System.Collections.Generic;
using System.Windows.Input;
using ChineseCharacterTrainer.Library;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.Implementation.ViewModels
{
    public interface IHighscoreVM : IViewModel
    {
        List<dynamic> Highscores { get; }
        int CurrentHighscore { get; }
        int PersonalBest { get; }

        ICommand ContinueCommand { get; }

        void Initialize(Highscore currentHighscore);

        event Action ReturnToMenuRequested;
    }
}