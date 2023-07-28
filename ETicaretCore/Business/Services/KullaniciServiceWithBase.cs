using AppCore.Business.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using DataAccess.EntityFramework.Repositories;

namespace Business.Services
{
    public interface IKullaniciService : IService<KullaniciModel>
    {

    }

    public class KullaniciService : IKullaniciService
    {
        private readonly KullaniciRepositoryBase _kullaniciRepository;

        public KullaniciService(KullaniciRepositoryBase kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }

        public Result Add(KullaniciModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _kullaniciRepository.Dispose();
        }

        public IQueryable<KullaniciModel> Query()
        {
            return _kullaniciRepository.EntityQuery("Rol", "KullaniciDetayi").Select(k => new KullaniciModel()
            {
                Active = k.Active,
                KullaniciAdi = k.KullaniciAdi,
                Sifre = k.Sifre,
                RolId = k.RolId,
                KullaniciDetayiId = k.KullaniciDetayiId,

                RolModel = new RolModel()
                {
                    Adi = k.Rol.Adi
                },
                KullaniciDetayiModel = new KullaniciDetayiModel()
                {
                    EPosta = k.KullaniciDetayi.EPosta,
                    Adres = k.KullaniciDetayi.Adres
                }
            });
        }

        public Result Update(KullaniciModel model)
        {
            throw new NotImplementedException();
        }
    }
}
