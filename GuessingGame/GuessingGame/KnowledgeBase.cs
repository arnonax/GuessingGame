using System.Data.SQLite;

namespace GuessingGame
{
    public interface IKnowledgeBase
    {
        IFact Root { get; }
    }

    public class KnowledgeBase : IKnowledgeBase
    {
        public const string DataFilename = "Knowledge.sqlite";

        private static KnowledgeBase s_instance;
        private bool _isRootLoaded;

        private long _rootId;

        public static KnowledgeBase Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new KnowledgeBase();

                return s_instance;
            }
        }

        public IFact Root
        {
            get
            {
                if (!_isRootLoaded)
                {
                    const string sql = "select id from facts where parentId is null";
                    _rootId = ExecuteCommand<long>(sql);
                }

                return new Fact(_rootId);
            }
        }

        // TODO: move to another class
        public void InitBasicData()
        {
            CreateDatabase();

            _rootId = InsertRootFact("animal");
            InsertFact("cat", _rootId, true);
            InsertFact("house", _rootId, false);

            _isRootLoaded = true;
        }

        public void CreateDatabase()
        {
            SQLiteConnection.CreateFile(DataFilename);
            const string createTableSql = @"
create table facts (
    id integer primary key autoincrement,
    parentId integer,
    isParentsCorrectAnswer integer,
    description varchar(100))";
            ExecuteCommand(createTableSql);
        }

        public long InsertRootFact(string description)
        {
            const string sql = "insert into facts (description) values (?); select last_insert_rowid() from facts";
            return ExecuteCommand<long>(sql, description);
        }

        public long InsertFact(string description, long parentId, bool isParentsCorrectAnswer)
        {
            const string sql = @"
insert into facts 
    (description, parentId, isParentsCorrectAnswer) 
    values (?, ?, ?); 
select last_insert_rowid() from facts";
            return ExecuteCommand<long>(sql, description, parentId, isParentsCorrectAnswer ? 1 : 0);
        }

        // TODO: move to another class
        public void ExecuteCommand(string sql, params object[] arguments)
        {
            using (var connection = new SQLiteConnection($"Data Source={DataFilename}"))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    foreach (var argument in arguments)
                    {
                        //command.Parameters.Add(argument, GetDbType(argument.GetType()));
                        command.Parameters.Add(new SQLiteParameter {Value = argument});
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        // TODO: move to another class
        public T ExecuteCommand<T>(string sql, params object[] arguments)
        {
            using (var connection = new SQLiteConnection($"Data Source={DataFilename}"))
            {
                connection.Open();
                using (var command = new SQLiteCommand(sql, connection))
                {
                    foreach (var argument in arguments)
                    {
                        command.Parameters.Add(new SQLiteParameter {Value = argument});
                    }
                    return (T)command.ExecuteScalar();
                }
            }
        }

        public string GetDescriptionByRowId(long rowId)
        {
            const string sql = "select description from facts where id=?";
            return ExecuteCommand<string>(sql, rowId);
        }

        public long? GetChildFact(long parentRowId, bool isParentsCorrectAnswer)
        {
            const string sql = "select id from facts where parentId = ? and isParentsCorrectAnswer = ?";
            return ExecuteCommand<long?>(sql, parentRowId, isParentsCorrectAnswer ? 1 : 0);
        }

        public void UpdateDescription(long rowId, string newDescription)
        {
            const string sql = @"
update facts
set description = ?
where id = ?";
            ExecuteCommand(sql, newDescription, rowId);
        }
    }
}