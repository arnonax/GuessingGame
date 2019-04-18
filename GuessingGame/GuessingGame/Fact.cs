namespace GuessingGame
{
    public interface IFact
    {
        string GetQuestion();
        IFact GetChild(Answer answer);
        void InsertChild(string property, string description);
        string Description { get; }
    }

    internal class Fact : IFact
    {
        private readonly long _id;

        public Fact(long id)
        {
            _id = id;
        }

        public string Description
        {
            get
            {
                return
                    KnowledgeBase.Instance.GetDescriptionByRowId(_id);
            }
            set { KnowledgeBase.Instance.UpdateDescription(_id, value); }
        }

        public string GetQuestion()
        {
            return $"Is it {TextUtils.GetArticle(Description)}?";
        }

        public IFact GetChild(Answer answer)
        {
            var childId = KnowledgeBase.Instance.GetChildFact(_id, answer == Answer.Yes);
            return childId != null ? new Fact(childId.Value) : null;
        }

        public void InsertChild(string property, string description)
        {
            KnowledgeBase.Instance.InsertFact(description, _id, true);
            KnowledgeBase.Instance.InsertFact(Description, _id, false);
            Description = property;
        }
    }
}