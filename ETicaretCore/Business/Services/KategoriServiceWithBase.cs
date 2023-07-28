using AppCore.Business.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using DataAccess.EntityFramework.Repositories;

namespace Business.Services
{
    public interface IKategoriService : IService<KategoriModel>
    {

    }

    public class KategoriService : IKategoriService
    {
        private readonly KategoriRepositoryBase _kategoriRepository;

        public KategoriService(KategoriRepositoryBase kategoriRepository)
        {
            _kategoriRepository = kategoriRepository;
        }

        public Result Add(KategoriModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _kategoriRepository.Dispose();
        }

        public IQueryable<KategoriModel> Query()
        {
            return _kategoriRepository.EntityQuery().OrderBy(k => k.Adi).Select(k => new KategoriModel()
            {
                Id = k.Id,
                Adi = k.Adi
            });
        }

        public Result Update(KategoriModel model)
        {
            throw new NotImplementedException();
        }
    }
}
