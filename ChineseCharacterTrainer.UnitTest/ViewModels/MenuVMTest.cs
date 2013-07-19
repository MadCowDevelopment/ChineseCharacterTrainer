using System.Threading;
using ChineseCharacterTrainer.Implementation.Services;
using ChineseCharacterTrainer.Implementation.ViewModels;
using ChineseCharacterTrainer.Library;
using ChineseCharacterTrainer.Model;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChineseCharacterTrainer.UnitTest.ViewModels
{
    public class MenuVMTest
    {
        private IMenuVM _objectUnderTest;
        private Mock<IOpenFileDialog> _openFileDialogMock;
        private Mock<IDictionaryImporter> _dictionaryImporterMock;
        private Mock<IRepository> _dictionaryRepositoryMock;

        [SetUp]
        public void Initialize()
        {
            _openFileDialogMock = new Mock<IOpenFileDialog>();
            _openFileDialogMock.SetupAllProperties();

            _dictionaryImporterMock = new Mock<IDictionaryImporter>();
            _dictionaryRepositoryMock = new Mock<IRepository>();
            _dictionaryRepositoryMock.Setup(p => p.GetAllDictionaries()).Returns(new List<Dictionary>());

            _objectUnderTest = CreateObjectUnderTest();
            _objectUnderTest.Initialize();
        }

        private IMenuVM CreateObjectUnderTest()
        {
            return new MenuVM(
                _openFileDialogMock.Object, _dictionaryImporterMock.Object, _dictionaryRepositoryMock.Object);
        }

        [Test]
        public void ShouldSetFilterToCsv()
        {
            Assert.AreEqual("Comma separated files (*.csv)|*.csv|All files (*.*)|*.*", _openFileDialogMock.Object.Filter);
        }

        [Test]
        public void ShouldImportFileWhenUserAcceptsFile()
        {
            _objectUnderTest.Name = "MyDict";
            _objectUnderTest.FileName = "somefile.csv";

            _objectUnderTest.ImportCommand.Execute(null);

            _dictionaryImporterMock.Verify(p => p.ImportAsync("MyDict", "somefile.csv"));
        }

        [Test]
        public void ShouldBeAbleToImportWhenNameAndFileNameAreNotNull()
        {
            _objectUnderTest.Name = "MyDict";
            _objectUnderTest.FileName = "somefile.csv";

            var canExecute = _objectUnderTest.ImportCommand.CanExecute(null);

            Assert.IsTrue(canExecute);
        }

        [Test]
        public void ShouldNotBeAbleToImportWhenNameIsNull()
        {
            _objectUnderTest.FileName = "somefile.csv";

            var canExecute = _objectUnderTest.ImportCommand.CanExecute(null);

            Assert.IsFalse(canExecute);
        }

        [Test]
        public void ShouldNotBeAbleToImportWhenFileNameIsNull()
        {
            _objectUnderTest.Name = "MyDict";

            var canExecute = _objectUnderTest.ImportCommand.CanExecute(null);

            Assert.IsFalse(canExecute);
        }
        
        [Test]
        public void ShouldResetSelectionsWhenImportIsStarted()
        {
            _objectUnderTest.Name = "MyDict";
            _objectUnderTest.FileName = "somefile.csv";

           _objectUnderTest.ImportCommand.Execute(null);

            Assert.AreEqual(string.Empty, _objectUnderTest.Name);
            Assert.AreEqual(string.Empty, _objectUnderTest.FileName);
        }

        [Test]
        public async void ShouldAddDictionaryToAvailableDictionariesWhenImportingFile()
        {
            _objectUnderTest.Name = "MyDict";
            _objectUnderTest.FileName = "somefile.csv";
            var task = new Task<Dictionary>(() =>
                {
                    Thread.Sleep(10);
                    return new Dictionary("MyDict", null);
                });
            _dictionaryImporterMock.Setup(p => p.ImportAsync(_objectUnderTest.Name, _objectUnderTest.FileName))
                                   .Returns(task);
            task.Start();

            await ((RelayCommand)_objectUnderTest.ImportCommand).ExecuteAsync(null);

            Assert.AreEqual(1, _objectUnderTest.AvailableDictionaries.Count);
        }

        [Test]
        public async void ShouldInitializeAvailableDictionariesFromRepository()
        {
            var dictionaries = new List<Dictionary> {new Dictionary("1", null), new Dictionary("2", null)};
            _dictionaryRepositoryMock.Setup(p => p.GetAllDictionaries()).Returns(dictionaries);

            _objectUnderTest = CreateObjectUnderTest();
            await _objectUnderTest.Initialize();

            Assert.AreEqual(2, _objectUnderTest.AvailableDictionaries.Count);
        }

        [Test]
        public void ShouldBeAbleToStartCompetitionWhenDictionaryIsSelected()
        {
            _objectUnderTest.SelectedDictionary = new Dictionary("1", null);

            var canOpen = _objectUnderTest.StartCompetitionCommand.CanExecute(null);

            Assert.IsTrue(canOpen);
        }

        [Test]
        public void ShouldNotBeAbleToStartCompetitionWhenNoDictionaryIsSelected()
        {
            _objectUnderTest.SelectedDictionary = null;

            var canOpen = _objectUnderTest.StartCompetitionCommand.CanExecute(null);

            Assert.IsFalse(canOpen);
        }

        [Test]
        public void ShouldNotBeAbleToStartPracticeWhenNoDictionaryIsSelected()
        {
            _objectUnderTest.SelectedDictionary = null;

            var canOpen = _objectUnderTest.StartPracticeCommand.CanExecute(null);

            Assert.IsFalse(canOpen);
        }

        [Test]
        public void ShouldRaiseEventWhenCompetitionShouldBeStarted()
        {
            _objectUnderTest.SelectedDictionary = new Dictionary("1", null);
            Dictionary dictionaryToOpen = null;
            _objectUnderTest.StartCompetitionRequested += dictionary => dictionaryToOpen = dictionary;

            _objectUnderTest.StartCompetitionCommand.Execute(null);

            Assert.AreEqual(_objectUnderTest.SelectedDictionary, dictionaryToOpen);
        }

        [Test]
        public void ShouldRaiseEventWhenPracticeShouldBeStarted()
        {
            _objectUnderTest.SelectedDictionary = new Dictionary("1", null);
            _dictionaryRepositoryMock.Setup(p => p.GetDictionaryEntriesForQueryObject(It.IsAny<QueryObject>()))
                                     .Returns(new List<DictionaryEntry> {new DictionaryEntry(null, null, null)});
            List<DictionaryEntry> dictionaryEntries = null;
            _objectUnderTest.StartPracticeRequested += args => dictionaryEntries = args;

            _objectUnderTest.StartPracticeCommand.Execute(null);

            Assert.IsNotNull(dictionaryEntries);
        }

        [Test]
        public void ShouldNotRaiseEventWhenNoItemsAreFoundForPractice()
        {
            _objectUnderTest.SelectedDictionary = new Dictionary("1", null);
            _dictionaryRepositoryMock.Setup(p => p.GetDictionaryEntriesForQueryObject(It.IsAny<QueryObject>()))
                                     .Returns(new List<DictionaryEntry>());
            List<DictionaryEntry> dictionaryEntries = null;
            _objectUnderTest.StartPracticeRequested += args => dictionaryEntries = args;

            _objectUnderTest.StartPracticeCommand.Execute(null);

            Assert.IsNull(dictionaryEntries);
        }

        [Test]
        public void ShouldOpenFileDialogWhenBrowseCommandExecutes()
        {
            _objectUnderTest.BrowseCommand.Execute(null);

            _openFileDialogMock.Verify(p => p.ShowDialog());
        }

        [Test]
        public void ShouldSetFileNameAfterUserAcceptsFile()
        {
            _openFileDialogMock.Setup(p => p.ShowDialog()).Returns(true);
            _openFileDialogMock.Setup(p => p.FileName).Returns("somefile.csv");

            _objectUnderTest.BrowseCommand.Execute(null);

            Assert.AreEqual("somefile.csv", _objectUnderTest.FileName);
        }

        [Test]
        public void ShouldNotSetFileNameAfterUserCancelsFileDialog()
        {
            _openFileDialogMock.Setup(p => p.ShowDialog()).Returns(false);

            _objectUnderTest.BrowseCommand.Execute(null);

            Assert.AreEqual(null, _objectUnderTest.FileName);
        }
    }
}
