using System.ComponentModel;
using GuessingGame;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuessingGameTests
{
    [TestClass]
    public class IntegrationTests : GameEngineAbstractTests<KnowledgeBase>
    {
        public IntegrationTests() 
            : base(KnowledgeBase.Instance)
        {
        }

        protected override void SetKnowledge(DummyFact knowledgeTree)
        {
            KnowledgeBase.Instance.CreateDatabase();
            AddFacts(knowledgeTree);
        }

        private void AddFacts(IFact fact)
        {
            var rootId = KnowledgeBase.Instance.InsertRootFact(fact.Description);
            AddChildren(fact, rootId);
        }

        private void AddFact(IFact fact, long parentId, bool isParentsCorrectAnswer)
        {
            if (fact == null)
                return;

            var id = KnowledgeBase.Instance.InsertFact(fact.Description, parentId, isParentsCorrectAnswer);
            AddChildren(fact, id);
        }

        private void AddChildren(IFact fact, long id)
        {
            var incorrectAnserChild = fact.GetChild(Answer.No);
            AddFact(incorrectAnserChild, id, false);

            var correctAnswerChild = fact.GetChild(Answer.Yes);
            AddFact(correctAnswerChild, id, true);
        }

        protected override void AssertFactWasAdded(IFact houseFact, string property, string description)
        {
            throw new System.NotImplementedException();
        }

        protected override IFact CreateFakeFact(string description)
        {
            return new DummyFact(description);
        }
    }
}
