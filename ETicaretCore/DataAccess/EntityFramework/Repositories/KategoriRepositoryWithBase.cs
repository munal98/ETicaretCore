using AppCore.DataAccess.Bases.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework.Repositories
{

    public abstract class KategoriRepositoryBase : RepositoryBase<Kategori>
    {
        protected KategoriRepositoryBase(DbContext db) : base(db)
        {

        }
    }

    public class KategoriRepository : KategoriRepositoryBase
    {
        public KategoriRepository(DbContext db) : base(db)
        {

        }
    }
}
