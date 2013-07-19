using System;
using System.Data.Entity;
using System.Linq;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.ServiceApp.Persistence
{
    public class ChineseTrainerContext : DbContext, IChineseTrainerContext
    {
        public ChineseTrainerContext(string databaseName)
            : base("data source=localhost;initial catalog=" + databaseName + ";integrated security=True;multipleactiveresultsets=True;App=EntityFramework")
        {
        }

        public ChineseTrainerContext() { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new DictionaryEntryMapping());
            modelBuilder.Configurations.Add(new DictionaryMapping());
            modelBuilder.Configurations.Add(new TranslationMapping());
            modelBuilder.Configurations.Add(new HighscoreMapping());
            modelBuilder.Configurations.Add(new QuestionResultMapping());
            modelBuilder.Configurations.Add(new AnswerMapping());
        }

        public IQueryable<T> GetAll<T>() where T : Entity
        {
            return Set<T>();
        }

        public void Add<T>(T entity) where T : Entity
        {
            Set<T>().Add(entity);
        }

        public T GetById<T>(Guid id) where T : Entity
        {
            return Set<T>().FirstOrDefault(p => p.Id == id);
        }
    }
}
