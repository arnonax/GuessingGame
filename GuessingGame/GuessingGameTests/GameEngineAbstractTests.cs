using FakeItEasy;
using GuessingGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuessingGameTests
{
    public abstract class GameEngineAbstractTests<TKnowledgeBase>
        where TKnowledgeBase : IKnowledgeBase
    {
        protected readonly IUserInterface _user;
        protected readonly GameEngine _engine;
        protected readonly TKnowledgeBase _knowledgeBase;

        protected GameEngineAbstractTests(TKnowledgeBase knowledgeBase)
        {
            _user = A.Fake<IUserInterface>();
            _knowledgeBase = knowledgeBase;
            _engine = new GameEngine(_knowledgeBase, _user);
        }

        [TestMethod]
        public void ComputerWinsWhenUserAnswersYesToAllQuestions()
        {
            var computerWon = false;
            _engine.OnWin += ()=> computerWon = true;
            var knowledgeTree = new DummyFact("animal",
                new DummyFact("cat"),
                new DummyFact("house"));
            SetKnowledge(knowledgeTree);
            A.CallTo(() => _user.GetYesNoAnswer()).Returns(Answer.Yes);
            _engine.Play();
            Assert.IsTrue(computerWon, "Computer should win");
        }

        protected abstract void SetKnowledge(DummyFact knowledgeTree);

        [TestMethod]
        public void WhenComputerLooseItLearnsNewFact()
        {
            var computerWon = false;
            _engine.OnWin += () => computerWon = true;

            var houseFact = CreateFakeFact("house");
            SetKnowledge(new DummyFact("animal",
                new DummyFact("cat"),
                houseFact));
            A.CallTo(() => _user.GetYesNoAnswer())
                .Returns(Answer.No);
            var property = "barking";
            var description = "dog";
            A.CallTo(() => _user.AskQuestion(A<string>._))
                .Returns(description);
            A.CallTo(() => _user.AskUserToComplete(A<string>._, A<string>._))
                .Returns(property);

            _engine.Play();

            AssertFactWasAdded(houseFact, property, description);

            Assert.IsFalse(computerWon);
        }

        protected abstract void AssertFactWasAdded(IFact houseFact, string property, string description);
        protected abstract IFact CreateFakeFact(string description);
    }
}