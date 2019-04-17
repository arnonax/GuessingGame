using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuessingGameTests
{
    [TestClass]
    public class GameEngineTests
    {
        //private readonly MemoryKnowledgeBase _knowledgeBase;
        //private readonly IUserInterface _user;
        //private readonly GameEngine _engine;

        private GameEngineTests()
        {
            //_knowledgeBase = new MemoryKnowledgeBase();
            //_user = A.Fake<IUserInterface>();
            //_engine = new GameEngine(_knowledgeBase, _user);
        }

        [TestMethod]
        public void ComputerWinsWhenUserAnswersYesToAllQuestions()
        {
            //bool computerWon;
            //_engine.OnWin += computerWon = true;
            //_knowledgeBase.SetData(new DummyFact("animal",
            //    new DummyFact("cat"),
            //    new DummyFact("house")));
            //A.CallTo(() => _user.GetAnswer(A<string>())).Returns(true);
            //_engine.Play();
            //Assert.IsTrue(computerWon, "Computer should win");
        }
    }
}
