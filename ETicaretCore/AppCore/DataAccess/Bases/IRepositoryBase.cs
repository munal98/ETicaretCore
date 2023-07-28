using AppCore.Records.Bases;

namespace AppCore.DataAccess.Bases
{
    //public interface IRepositoryBase<TEntity> where TEntity : class
    //public interface IRepositoryBase<TEntity> where TEntity : class, new()
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : RecordBase, new()
    {
        IQueryable<TEntity> Query(); // Read
        void Add(TEntity entity, bool save = true); // Create
        void Update(TEntity entity, bool save = true); // Update
        void Delete(TEntity entity, bool save = true); // Delete
        int Save(); // database commit
    }
}
