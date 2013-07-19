using System;
using System.Linq;
using ChineseCharacterTrainer.Model;

namespace ChineseCharacterTrainer.ServiceApp.Persistence
{
    public interface IChineseTrainerContext
    {
        int SaveChanges();
        IQueryable<T> GetAll<T>() where T : Entity;
        void Add<T>(T entity) where T : Entity;
        T GetById<T>(Guid id) where T : Entity;
    }
}