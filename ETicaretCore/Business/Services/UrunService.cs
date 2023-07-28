using AppCore.Business.Models.Ordering;
using AppCore.Business.Models.Paging;
using AppCore.Business.Results;
using Business.Models;
using Business.Models.Filters;
using Business.Models.Reports;
using Business.Services.Bases;
using DataAccess.EntityFramework.Repositories;
using DataAccess.EntityFramework.Repositories.Bases;
using Entities.Entities;
using System.Globalization;

namespace Business.Services
{
    public class UrunService : IUrunService
    {
        private readonly UrunRepositoryBase _urunRepository;
        private readonly KategoriRepositoryBase _kategoriRepository;

        public UrunService(UrunRepositoryBase urunRepository, KategoriRepositoryBase kategoryRepositoryBase)
        {
            _urunRepository = urunRepository;
            _kategoriRepository = kategoryRepositoryBase;
        }

        public Result Add(UrunModel model)
        {
            if (_urunRepository.EntityQuery().Any(u => u.Adi.ToLower() == model.Adi.ToLower().Trim()))
            {
                return new ErrorResult("Aynı ürün adına sahip kayıt bulunmaktadır!");
            }
            DateTime? sonKullanmaTarihi = string.IsNullOrWhiteSpace(model.SonKullanmaTarihiModel) ? null : DateTime.Parse(model.SonKullanmaTarihiModel, new CultureInfo("tr-TR"));
            if (sonKullanmaTarihi != null && sonKullanmaTarihi.Value <= DateTime.Today)
            {
                return new ErrorResult("Son kullanma tarihi yarın veya daha sonraki bir tarih olmalıdır!");
            }
            double birimFiyati = Convert.ToDouble(model.BirimFiyatiModel.Trim().Replace(",", "."), CultureInfo.InvariantCulture);
            if (!(birimFiyati >= 0 && birimFiyati <= 50000))
            {
                return new ErrorResult("Birim fiyatı 0 ile 50000 arasında olmalıdır!");
            }
            var entity = new Urun()
            {
                Adi = model.Adi.Trim(),
                Aciklamasi = model.Aciklamasi?.Trim(),
                BirimFiyati = birimFiyati,
                StokMiktari = model.StokMiktari,
                SonKullanmaTarihi = sonKullanmaTarihi,
                KategoriId = model.KategoriId,
                CreatedBy = model.CreatedBy,
                ImajDosyaAdi = model.ImajDosyaAdi
            };
            _urunRepository.Add(entity);
            model.Id = entity.Id;
            return new SuccessResult("Ürün başarıyla eklendi.");
        }

        public Result Delete(int id)
        {
            _urunRepository.DeleteEntity(id);
            return new SuccessResult("Ürün başarıyla silindi.");
        }

        public void Dispose()
        {
            _urunRepository.Dispose();
        }

        public IQueryable<UrunModel> Query()
        {
            var query = _urunRepository.EntityQuery("Kategori").OrderBy(u => u.Adi).Select(u => new UrunModel()
            {
                Id = u.Id,
                Adi = u.Adi,
                Aciklamasi = u.Aciklamasi,
                BirimFiyati = u.BirimFiyati,
                StokMiktari = u.StokMiktari,
                SonKullanmaTarihi = u.SonKullanmaTarihi,
                KategoriId = u.KategoriId,

                BirimFiyatiModel = u.BirimFiyati.ToString(new CultureInfo("tr-TR")),
                SonKullanmaTarihiModel = u.SonKullanmaTarihi.HasValue ? u.SonKullanmaTarihi.Value.ToString("yyyy-MM-dd") : "",
                KategoriModel = new KategoriModel()
                {
                    Id = u.Kategori.Id,
                    Adi = u.Kategori.Adi,
                    Aciklamasi = u.Kategori.Aciklamasi
                },
                ImajDosyaAdi = u.ImajDosyaAdi
            });
            return query;
        }

        public Result Update(UrunModel model)
        {
            if (_urunRepository.EntityQuery().Any(u => u.Adi.ToLower() == model.Adi.ToLower().Trim() && u.Id != model.Id))
            {
                return new ErrorResult("Aynı ürün adına sahip kayıt bulunmaktadır!");
            }
            DateTime? sonKullanmaTarihi = string.IsNullOrWhiteSpace(model.SonKullanmaTarihiModel) ? null : DateTime.Parse(model.SonKullanmaTarihiModel, new CultureInfo("tr-TR"));
            if (sonKullanmaTarihi != null && sonKullanmaTarihi.Value <= DateTime.Today)
            {
                return new ErrorResult("Son kullanma tarihi yarın veya daha sonraki bir tarih olmalıdır!");
            }
            var entity = _urunRepository.EntityQuery().SingleOrDefault(u => u.Id == model.Id);
            entity.Adi = model.Adi.Trim();
            entity.Aciklamasi = model.Aciklamasi?.Trim();
            entity.BirimFiyati = Convert.ToDouble(model.BirimFiyatiModel.Trim().Replace(",", "."), CultureInfo.InvariantCulture);
            entity.StokMiktari = model.StokMiktari;
            entity.SonKullanmaTarihi = sonKullanmaTarihi;
            entity.KategoriId = model.KategoriId;
            entity.UpdatedBy = model.UpdatedBy;
            entity.ImajDosyaAdi = model.ImajDosyaAdi;
            _urunRepository.Update(entity);
            return new SuccessResult("Ürün başarıyla güncellendi.");
        }

        /*
        select u.Adi [Ürün Adı],
        k.Adi [Kategori Adı],
        k.Aciklamasi [Kategori Açıklaması],
        u.BirimFiyati [Birim Fiyat],
        u.StokMiktari [Stok Miktarı],
        convert(varchar(10), u.SonKullanmaTarihi, 104) [Son Kullanma Tarihi] 
        from ETicaretUrunler u
        inner join ETicaretKategoriler k
        on u.KategoriId = k.Id
        */
        public Result<List<UrunRaporuModel>> UrunRaporuGetir(UrunRaporuFiltreModel filtre = null, PageModel sayfa = null, OrderModel sira = null)
        {
            #region Query
            var urunQuery = _urunRepository.EntityQuery();
            var kategoriQuery = _kategoriRepository.EntityQuery();

            //var query = from u in urunQuery
            //            join k in kategoriQuery
            //            on u.KategoriId equals k.Id
            //            //orderby k.Adi, u.Adi
            //            //where u.Id == 5
            //            select new UrunRaporuModel()
            //            {
            //                BirimFiyatiModel = u.BirimFiyati.ToString("C2", new CultureInfo("tr-TR")),
            //                CreateDateModel = u.CreateDate.HasValue ? u.CreateDate.Value.ToString(new CultureInfo("tr-TR")) : "",
            //                UpdateDateModel = u.UpdateDate.HasValue ? u.UpdateDate.Value.ToString(new CultureInfo("tr-TR")) : "",
            //                CreatedByModel = u.CreatedBy,
            //                UpdatedByModel = u.UpdatedBy,
            //                KategoriAciklamasi = k.Aciklamasi,
            //                KategoriAdi = k.Adi,
            //                SonKullanmaTarihiModel = u.SonKullanmaTarihi.HasValue ? u.SonKullanmaTarihi.Value.ToString("dd.MM.yyyy", new CultureInfo("tr-TR")) : "",
            //                StokMiktari = u.StokMiktari,
            //                UrunAdi = u.Adi,
            //                KategoriId = k.Id,
            //                BirimFiyat = u.BirimFiyati,
            //                SonKullanmaTarihi = u.SonKullanmaTarihi
            //            };
            var query = from k in kategoriQuery
                        join u in urunQuery
                        on k.Id equals u.KategoriId into kategoriUrunJoin
                        from subKategoriUrunJoin in kategoriUrunJoin.DefaultIfEmpty()
                        //orderby k.Adi, subKategoriUrunJoin.Adi
                        select new UrunRaporuModel()
                        {
                            BirimFiyatiModel = subKategoriUrunJoin.BirimFiyati.ToString("C2", new CultureInfo("tr-TR")),
                            CreateDateModel = subKategoriUrunJoin.CreateDate.HasValue ? subKategoriUrunJoin.CreateDate.Value.ToString(new CultureInfo("tr-TR")) : "",
                            UpdateDateModel = subKategoriUrunJoin.UpdateDate.HasValue ? subKategoriUrunJoin.UpdateDate.Value.ToString(new CultureInfo("tr-TR")) : "",
                            CreatedByModel = subKategoriUrunJoin.CreatedBy,
                            UpdatedByModel = subKategoriUrunJoin.UpdatedBy,
                            KategoriAciklamasi = k.Aciklamasi,
                            KategoriAdi = k.Adi,
                            SonKullanmaTarihiModel = subKategoriUrunJoin.SonKullanmaTarihi.HasValue ? subKategoriUrunJoin.SonKullanmaTarihi.Value.ToString("dd.MM.yyyy", new CultureInfo("tr-TR")) : "",
                            StokMiktari = subKategoriUrunJoin.StokMiktari,
                            UrunAdi = subKategoriUrunJoin.Adi,
                            KategoriId = k.Id,
                            BirimFiyat = subKategoriUrunJoin.BirimFiyati,
                            SonKullanmaTarihi = subKategoriUrunJoin.SonKullanmaTarihi
                        };

            #region Ordering
            //query = query.OrderBy(q => q.KategoriAdi).ThenBy(q => q.UrunAdi);
            if (sira != null)
            {
                switch (sira.Expression)
                {
                    case "Kategori Adı": query = sira.DirectionAscending ? query.OrderBy(q => q.KategoriAdi) : query.OrderByDescending(q => q.KategoriAdi);
                        break;
                    case "Ürün Adı":
                        query = sira.DirectionAscending ? query.OrderBy(q => q.UrunAdi) : query.OrderByDescending(q => q.UrunAdi);
                        break;
                    case "Birim Fiyatı":
                        query = sira.DirectionAscending ? query.OrderBy(q => q.BirimFiyat) : query.OrderByDescending(q => q.BirimFiyat);
                        break;
                    case "Stok Miktarı":
                        query = sira.DirectionAscending ? query.OrderBy(q => q.StokMiktari) : query.OrderByDescending(q => q.StokMiktari);
                        break;
                    default: query = sira.DirectionAscending ? query.OrderBy(q => q.SonKullanmaTarihi) : query.OrderByDescending(q => q.SonKullanmaTarihi);
                        break;
                }
            }
            #endregion

            #region Filter
            if (filtre != null)
            {
                if (filtre.KategoriId != null)
                    query = query.Where(q => q.KategoriId == filtre.KategoriId);
                if (!string.IsNullOrWhiteSpace(filtre.UrunAdi))
                    query = query.Where(q => q.UrunAdi.ToUpper().Contains(filtre.UrunAdi.ToUpper().Trim()));
                if (!string.IsNullOrWhiteSpace(filtre.BirimFiyatBaslangic))
                {
                    double birimFiyatBaslangic = Convert.ToDouble(filtre.BirimFiyatBaslangic.Replace(",", "."), CultureInfo.InvariantCulture);
                    query = query.Where(q => q.BirimFiyat >= birimFiyatBaslangic);
                }
                if (!string.IsNullOrWhiteSpace(filtre.BirimFiyatBitis))
                {
                    double birimFiyatBitis = Convert.ToDouble(filtre.BirimFiyatBitis.Replace(",", "."), CultureInfo.InvariantCulture);
                    query = query.Where(q => q.BirimFiyat <= birimFiyatBitis);
                }
                if (!string.IsNullOrWhiteSpace(filtre.SonKullanmaTarihiBaslangic))
                {
                    DateTime sonKullanmaTarihiBaslangic = DateTime.Parse(filtre.SonKullanmaTarihiBaslangic, new CultureInfo("tr-TR"));
                    query = query.Where(q => q.SonKullanmaTarihi >= sonKullanmaTarihiBaslangic);
                }
                if (!string.IsNullOrWhiteSpace(filtre.SonKullanmaTarihiBitis))
                {
                    DateTime sonKullanmaTarihiBitis = DateTime.Parse(filtre.SonKullanmaTarihiBitis, new CultureInfo("tr-TR"));
                    query = query.Where(q => q.SonKullanmaTarihi <= sonKullanmaTarihiBitis);
                }
                if (filtre.StokMiktariBaslangic != null)
                {
                    query = query.Where(q => q.StokMiktari >= filtre.StokMiktariBaslangic);
                }
                if (filtre.StokMiktariBitis != null)
                {
                    query = query.Where(q => q.StokMiktari <= filtre.StokMiktariBitis);
                }
            }
            #endregion

            #region Paging
            if (sayfa != null)
            {
                sayfa.RecordsCount = query.Count();
                int skip = (sayfa.PageNumber - 1) * sayfa.RecordsPerPageCount;
                int take = sayfa.RecordsPerPageCount;
                query = query.Skip(skip).Take(take);
            }
            #endregion

            return new SuccessResult<List<UrunRaporuModel>>(query.ToList());
            #endregion
        }
    }
}
