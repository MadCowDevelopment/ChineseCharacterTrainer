using System.Collections.Generic;
using System.Linq;
using ChineseCharacterTrainer.Implementation.Services;
using ChineseCharacterTrainer.Library;
using ChineseCharacterTrainer.Model;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChineseCharacterTrainer.Implementation.ViewModels
{
    public class MenuVM : ViewModel, IMenuVM
    {
        private readonly IOpenFileDialog _openFileDialog;
        private readonly IDictionaryImporter _dictionaryImporter;
        private readonly IRepository _repository;
        private IAsyncCommand _importCommand;
        private Dictionary _selectedDictionary;
        private ICommand _startCompetitionCommand;
        private string _name;
        private string _fileName;
        private ICommand _browseCommand;
        private ObservableCollection<Dictionary> _availableDictionaries;
        private ICommand _startPracticeCommand;

        public MenuVM(
            IOpenFileDialog openFileDialog,
            IDictionaryImporter dictionaryImporter,
            IRepository dictionaryRepository)
        {
            _openFileDialog = openFileDialog;
            _dictionaryImporter = dictionaryImporter;
            _repository = dictionaryRepository;
            _openFileDialog.Filter = "Comma separated files (*.csv)|*.csv|All files (*.*)|*.*";

            AvailableDictionaries = new ObservableCollection<Dictionary>();
        }

        public Dictionary SelectedDictionary
        {
            get { return _selectedDictionary; }
            set { _selectedDictionary = value; RaisePropertyChanged(() => SelectedDictionary); }
        }

        public ObservableCollection<Dictionary> AvailableDictionaries
        {
            get { return _availableDictionaries; }
            private set { _availableDictionaries = value; RaisePropertyChanged(() => AvailableDictionaries); }
        }

        public IAsyncCommand ImportCommand
        {
            get
            {
                return _importCommand ??
                       (_importCommand =
                        new RelayCommand(
                            async p =>
                                {
                                    var dictionaryName = Name;
                                    var fileName = FileName;
                                    Name = string.Empty;
                                    FileName = string.Empty;
                                    var dictionary = await _dictionaryImporter.ImportAsync(dictionaryName, fileName);
                                    AvailableDictionaries.Add(dictionary);
                                },
                            p => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(FileName)));
            }
        }

        public ICommand StartCompetitionCommand
        {
            get
            {
                return _startCompetitionCommand ??
                       (_startCompetitionCommand =
                        new RelayCommand(p =>
                                             {
                                                 var entries =
                                                     _repository.GetDictionaryEntriesByDictionaryId(
                                                         SelectedDictionary.Id);
                                                 RaiseStartCompetitionRequested(entries);
                                             },
                                         p => SelectedDictionary != null));
            }
        }

        public ICommand StartPracticeCommand
        {
            get
            {
                return _startPracticeCommand ??
                       (_startPracticeCommand = new RelayCommand(p =>
                           {
                               var queryObject = new QueryObject(SelectedDictionary.Id);
                               var entries = _repository.GetDictionaryEntriesForQueryObject(queryObject);
                               if (entries.Count == 0) return; // TODO: Show error.
                               RaiseStartPracticeRequested(entries);
                           }, p => SelectedDictionary != null));
            }
        }

        public ICommand BrowseCommand
        {
            get
            {
                return _browseCommand ??
                       (_browseCommand =
                        new RelayCommand(p =>
                            {
                                var result = _openFileDialog.ShowDialog();
                                if (result == true)
                                {
                                    FileName = _openFileDialog.FileName;
                                }
                            }));
            }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name); }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; RaisePropertyChanged(() => FileName); }
        }

        public event Action<List<DictionaryEntry>> StartPracticeRequested;

        public event Action<List<DictionaryEntry>> StartCompetitionRequested;

        public async Task Initialize()
        {
            var dictionaries = await Task.Run(() =>_repository.GetAllDictionaries());
            AvailableDictionaries = new ObservableCollection<Dictionary>(dictionaries);
            CommandManager.InvalidateRequerySuggested();
        }

        private void RaiseStartCompetitionRequested(List<DictionaryEntry> dictionaryEntries)
        {
            var handler = StartCompetitionRequested;
            if (handler != null) handler(dictionaryEntries);
        }

        protected virtual void RaiseStartPracticeRequested(List<DictionaryEntry> dictionaryEntries)
        {
            var handler = StartPracticeRequested;
            if (handler != null) handler(dictionaryEntries);
        }
    }
}
