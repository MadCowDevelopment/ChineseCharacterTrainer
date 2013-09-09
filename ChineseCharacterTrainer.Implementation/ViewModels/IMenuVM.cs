using System.Collections.Generic;
using System.Threading.Tasks;
using ChineseCharacterTrainer.Library;
using ChineseCharacterTrainer.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ChineseCharacterTrainer.Implementation.ViewModels
{
    public interface IMenuVM : IViewModel
    {
        Dictionary SelectedDictionary { get; set; }
        ObservableCollection<Dictionary> AvailableDictionaries { get; } 
        IAsyncCommand ImportCommand { get; }
        ICommand StartCompetitionCommand { get; }
        ICommand StartPracticeCommand { get; }
        ICommand BrowseCommand { get; }
        event Action<List<DictionaryEntry>> StartCompetitionRequested;
        event Action<List<DictionaryEntry>> StartPracticeRequested;
        string Name { get; set; }
        string FileName { get; set; }

        Task Initialize();
    }
}