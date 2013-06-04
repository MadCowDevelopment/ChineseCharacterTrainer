using System.Linq;
using ChineseCharacterTrainer.Model;
using System;
using System.Collections.Generic;

namespace ChineseCharacterTrainer.ServiceApp.Persistence
{
    public interface IChineseTrainerContext
    {
        int SaveChanges();
        //List<Entity> GetAll(Type type);
        //void Add(Type type, Entity entity);

        IQueryable<T> GetAll<T>() where T : class;
        void Add<T>(T entity) where T : class;
    }
}