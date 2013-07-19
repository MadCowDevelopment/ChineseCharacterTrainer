using ChineseCharacterTrainer.Implementation.Services;
using ChineseCharacterTrainer.Implementation.ViewModels;
using Moq;
using NUnit.Framework;

namespace ChineseCharacterTrainer.UnitTest.ViewModels
{
    public class PracticeSummaryVMTest
    {
        private PracticeSummaryVM _objectUnderTest;

        private Mock<IRepository> _repositoryMock;

        [SetUp]
        public void Initialize()
        {
            _repositoryMock = new Mock<IRepository>();
            _objectUnderTest = new PracticeSummaryVM(_repositoryMock.Object);
        }

        [Test]
        public void ShouldAddQuestionResultWhenReturningToMenu()
        {
            bool eventWasRaised = false;
            _objectUnderTest.ReturnToMenuRequested += () => eventWasRaised = true;

            _objectUnderTest.ReturnToMenuCommand.Execute(null);

            Assert.IsTrue(eventWasRaised);
        }
    }
}
