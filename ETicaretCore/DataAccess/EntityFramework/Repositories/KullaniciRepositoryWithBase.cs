using AppCore.DataAccess.Bases.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework.Repositories
{

    public abstract class KullaniciRepositoryBase : RepositoryBase<Kullanici>
    {
        protected KullaniciRepositoryBase(DbContext db) : base(db)
        {

        }
    }

    public class KullaniciRepository : KullaniciRepositoryBase
    {
        public KullaniciRepository(DbContext db) : base(db)
        {

        }
    }
}
