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
    }

    public class DummyFact : IFact
    {
        public DummyFact(string description, DummyFact factIfTrue, DummyFact factIfFalse)
        {
            throw new NotImplementedException();
        }

        public DummyFact(string description)
        {
            throw new NotImplementedException();
        }

        public string GetQuestion()
        {
            throw new NotImplementedException();
        }

        public IFact GetChild(Answer answer)
        {
            throw new NotImplementedException();
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
