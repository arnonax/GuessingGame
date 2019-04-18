using System;
using FakeItEasy;
using GuessingGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuessingGameTests
{
    [TestClass]
    public class GameEngineTests
    {
        private readonly MemoryKnowledgeBase _knowledgeBase;
        private readonly IUserInterface _user;
        private readonly GameEngine _engine;

        public GameEngineTests()
        {
            _knowledgeBase = new MemoryKnowledgeBase();
            _user = A.Fake<IUserInterface>();
            _engine = new GameEngine(_knowledgeBase, _user);
        }

        [TestMethod]
        public void ComputerWinsWhenUserAnswersYesToAllQuestions()
        {
            var computerWon = false;
            _engine.OnWin += ()=> computerWon = true;
            _knowledgeBase.Root = new DummyFact("animal",
                new DummyFact("cat"),
                new DummyFact("house"));
            A.CallTo(() => _user.GetYesNoAnswer()).Returns(Answer.Yes);
            _engine.Play();
            Assert.IsTrue(computerWon, "Computer should win");
        }

        [TestMethod]
        public void WhenComputerLooseItLearnsNewFact()
        {
            var computerWon = false;
            _engine.OnWin += () => computerWon = true;

            var houseFact = CreateFakeFact("house");
            _knowledgeBase.Root = new DummyFact("animal",
                new DummyFact("cat"),
                houseFact);
            A.CallTo(() => _user.GetYesNoAnswer())
                .Returns(Answer.No);
            A.CallTo(() => _user.AskQuestion(A<string>._))
                .Returns("dog");
            A.CallTo(() => _user.AskUserToComplete(A<string>._, A<string>._))
                .Returns("barking");

            _engine.Play();

            A.CallTo(() => houseFact.InsertChild("barking", "dog"))
                .MustHaveHappenedOnceExactly();

            Assert.IsFalse(computerWon);
        }

        private static IFact CreateFakeFact(string description)
        {
            var houseFact = A.Fake<IFact>();
            A.CallTo(() => houseFact.GetChild(A<Answer>._))
                .Returns(null);
            A.CallTo(() => houseFact.Description).Returns(description);
            return houseFact;
        }
    }

    public class DummyFact : IFact
    {
        private readonly IFact _factIfTrue;
        private readonly IFact _factIfFalse;

        public DummyFact(string description, IFact factIfTrue, IFact factIfFalse)
        {
            _factIfTrue = factIfTrue;
            _factIfFalse = factIfFalse;
        }

        public DummyFact(string description)
        {
            
        }

        public string GetQuestion()
        {
            return null;
        }

        public IFact GetChild(Answer answer)
        {
            return answer == Answer.Yes ? _factIfTrue : _factIfFalse;
        }

        public void InsertChild(string property, string description)
        {
            throw new NotImplementedException();
        }

        public string Description
        {
            get { throw new NotImplementedException(); }
        }
    }

    internal class MemoryKnowledgeBase : IKnowledgeBase
    {
        public IFact Root { get; set; }
    }
}
