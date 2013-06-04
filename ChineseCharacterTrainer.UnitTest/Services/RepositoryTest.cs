using System;
using ChineseCharacterTrainer.Implementation.ServiceReference;
using ChineseCharacterTrainer.Implementation.Services;
using ChineseCharacterTrainer.Model;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace ChineseCharacterTrainer.UnitTest.Services
{
    public class RepositoryTest
    {
        private IRepository _objectUnderTest;
        private Mock<IChineseCharacterTrainerService> _serviceMock;

        private Dictionary _dictionary = new Dictionary("MyDict", null);

        [SetUp]
        public void Initialize()
        {
            _serviceMock = new Mock<IChineseCharacterTrainerService>();

            _objectUnderTest = new Repository(_serviceMock.Object);
        }

        [Test]
        public void ShouldAddItemToServiceWhenAddingDictionary()
        {
            _objectUnderTest.Add(_dictionary);

            _serviceMock.Verify(p => p.Add(typeof(Dictionary).AssemblyQualifiedName, _dictionary));
        }

        [Test]
        public void ShouldGetDictionary()
        {
            _serviceMock.Setup(p => p.GetAll(typeof(Dictionary).AssemblyQualifiedName)).Returns(new List<Entity>
                {
                    new Dictionary("1", null),
                    new Dictionary("2", null)
                });

            var dictionaries = _objectUnderTest.GetAll<Dictionary>();

            Assert.AreEqual(2, dictionaries.Count);
        }

        [Test]
        public void ShouldAddHighscoreToServiceWhenAddingHighscore()
        {
            var highscore = CreateTestHighscore();

            _objectUnderTest.Add(highscore);

            _serviceMock.Verify(p => p.Add(typeof(Highscore).AssemblyQualifiedName, highscore));
        }

        [Test]
        public void ShouldGetHighscore()
        {
            _serviceMock.Setup(p => p.GetAll(typeof(Highscore).AssemblyQualifiedName)).Returns(new List<Entity>
                {
                    CreateTestHighscore(),
                    CreateTestHighscore()
                });

            var highscores = _objectUnderTest.GetAll<Highscore>();

            Assert.AreEqual(2, highscores.Count);
        }

        private Highscore CreateTestHighscore()
        {
            return new Highscore("Frank", _dictionary, new QuestionResult(0, 0, TimeSpan.FromSeconds(1)));
        }
    }
}
