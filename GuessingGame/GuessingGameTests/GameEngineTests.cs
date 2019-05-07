using System;
using FakeItEasy;
using GuessingGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuessingGameTests
{
    [TestClass]
    public class GameEngineTests : GameEngineAbstractTests<MemoryKnowledgeBase>
    {
        public GameEngineTests()
            : base(new MemoryKnowledgeBase())
        {
        }

        protected override void SetKnowledge(DummyFact knowledgeTree)
        {
            _knowledgeBase.Root = knowledgeTree;
        }

        protected override void AssertFactWasAdded(IFact houseFact, string property, string description)
        {
            A.CallTo(() => houseFact.InsertChild(property, description))
                .MustHaveHappenedOnceExactly();
        }

        protected override IFact CreateFakeFact(string description)
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

    public class MemoryKnowledgeBase : IKnowledgeBase
    {
        public IFact Root { get; set; }
    }
}
