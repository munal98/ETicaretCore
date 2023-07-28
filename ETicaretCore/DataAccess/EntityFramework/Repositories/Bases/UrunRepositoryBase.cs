using AppCore.DataAccess.Bases.EntityFramework;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework.Repositories.Bases
{
    public abstract class UrunRepositoryBase : RepositoryBase<Urun>
    {
        protected UrunRepositoryBase(DbContext db) : base(db)
        {

        }
    }
}
