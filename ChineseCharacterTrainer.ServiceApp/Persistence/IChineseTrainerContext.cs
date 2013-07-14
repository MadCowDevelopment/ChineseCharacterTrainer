using System.Linq;

namespace ChineseCharacterTrainer.ServiceApp.Persistence
{
    public interface IChineseTrainerContext
    {
        int SaveChanges();
        IQueryable<T> GetAll<T>() where T : class;
        void Add<T>(T entity) where T : class;
    }
}