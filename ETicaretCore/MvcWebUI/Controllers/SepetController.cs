using Business.Models;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "Kullanıcı")]
    public class SepetController : Controller
    {
        private readonly IUrunService _urunService;

        public SepetController(IUrunService urunService)
        {
            _urunService = urunService;
        }

        public IActionResult Ekle(int? urunId)
        {
            if (urunId == null)
            {
                return View("Hata", "Ürün ID boş olamaz!");
            }
            List<SepetModel> sepet = new List<SepetModel>();
            string sepetJson;
            var urun = _urunService.Query().SingleOrDefault(u => u.Id == urunId.Value);
            string kullaniciId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value;
            if (HttpContext.Session.GetString("sepet") != null)
            {
                sepetJson = HttpContext.Session.GetString("sepet");
                sepet = JsonConvert.DeserializeObject<List<SepetModel>>(sepetJson);
            }
            sepet.Add(new SepetModel()
            {
                UrunId = urunId.Value,
                UrunAdi = urun.Adi,
                BirimFiyati = urun.BirimFiyati,
                KullaniciId = Convert.ToInt32(kullaniciId)
            });
            sepetJson = JsonConvert.SerializeObject(sepet);
            HttpContext.Session.SetString("sepet", sepetJson);
            return RedirectToAction("Index", "Urunler");
        }

        public IActionResult Getir()
        {
            List<SepetModel> sepet = new List<SepetModel>();
            if (HttpContext.Session.GetString("sepet") != null)
            {
                sepet = JsonConvert.DeserializeObject<List<SepetModel>>(HttpContext.Session.GetString("sepet"));
            }
            List<SepetGroupByModel> sepetGroupByModel = (from s in sepet
                                                             //group s by s.UrunAdi
                                                         group s by new { s.UrunId, s.KullaniciId, s.UrunAdi }
                                                        into sGroupBy
                                                         select new SepetGroupByModel()
                                                         {
                                                             UrunId = sGroupBy.Key.UrunId,
                                                             KullaniciId = sGroupBy.Key.KullaniciId,
                                                             UrunAdi = sGroupBy.Key.UrunAdi,
                                                             BirimFiyatToplami = sGroupBy.Sum(s => s.BirimFiyati),
                                                             ToplamUrunSayisi = sGroupBy.Count()
                                                         }).ToList();
            return View(sepetGroupByModel);
        }

        public IActionResult Temizle()
        {
            HttpContext.Session.Remove("sepet");
            return RedirectToAction(nameof(Getir));
        }

        public IActionResult Sil(int urunId, int kullaniciId)
        {
            List<SepetModel> sepet = null;
            if (HttpContext.Session.GetString("sepet") != null)
            {
                sepet = JsonConvert.DeserializeObject<List<SepetModel>>(HttpContext.Session.GetString("sepet"));
            }
            if (sepet != null)
            {
                var sepetItem = sepet.FirstOrDefault(s => s.UrunId == urunId && s.KullaniciId == kullaniciId);
                sepet.Remove(sepetItem);
                var sepetJson = JsonConvert.SerializeObject(sepet);
                HttpContext.Session.SetString("sepet", sepetJson);
            }
            return RedirectToAction(nameof(Getir));
        }
    }
}
