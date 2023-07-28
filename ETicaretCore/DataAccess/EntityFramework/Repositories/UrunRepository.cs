using DataAccess.EntityFramework.Repositories.Bases;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EntityFramework.Repositories
{
    public class UrunRepository : UrunRepositoryBase
    {
        public UrunRepository(DbContext db) : base(db)
        {

        }
    }
}
