using AppCore.Business.Results;
using Business.Models;

namespace Business.Services
{
    public interface IHesapService
    {
        Result<KullaniciModel> Giris(KullaniciGirisModel model);

    }

    public class HesapService : IHesapService
    {
        private readonly IKullaniciService _kullaniciService;

        public HesapService(IKullaniciService kullaniciService)
        {
            _kullaniciService = kullaniciService;
        }

        public Result<KullaniciModel> Giris(KullaniciGirisModel kullaniciGiris)
        {
            var kullanici = _kullaniciService.Query().SingleOrDefault(k => k.KullaniciAdi == kullaniciGiris.KullaniciAdi && k.Sifre == kullaniciGiris.Sifre && k.Active);
            if (kullanici != null)
                return new SuccessResult<KullaniciModel>(kullanici);
            return new ErrorResult<KullaniciModel>("Kullanıcı bulunamadı!");
        }
    }
}
