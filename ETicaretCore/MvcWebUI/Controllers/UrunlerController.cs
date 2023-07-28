#nullable disable
using Business.Models;
using Business.Services;
using Business.Services.Bases;
using DataAccess.EntityFramework.Contexts;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcWebUI.Settings;

namespace MvcWebUI.Controllers
{
    [Authorize]
    public class UrunlerController : Controller
    {
        private readonly ETicaretContext _context;

        //public UrunlerController(ETicaretContext context)
        //{
        //    _context = context;
        //}

        private readonly IUrunService _urunService;
        private readonly IKategoriService _kategoriService;

        public UrunlerController(IUrunService urunService, IKategoriService kategoriService)
        {
            _urunService = urunService;
            _kategoriService = kategoriService;
        }

        // GET: Urunler
        //public IActionResult Index()
        //{
        //    var eTicaretContext = _context.Urunler.Include(u => u.Kategori);
        //    return View(eTicaretContext.ToList());
        //}

        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = _urunService.Query().ToList();
            return View(model);
        }


        // GET: Urunler/Details/5
        //public IActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var urun = _context.Urunler
        //        .Include(u => u.Kategori)
        //        .SingleOrDefault(m => m.Id == id);
        //    if (urun == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(urun);
        //}
        //[Authorize(Roles = "Admin,Kullanici")]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Hata", "Id gereklidir!");
            }
            var urun = _urunService.Query().SingleOrDefault(u => u.Id == id.Value);
            if (urun == null)
            {
                return View("Hata", "Ürün bulunamadı!");
            }
            return View(urun);
        }

        // GET: Urunler/Create
        //public IActionResult Create()
        //{
        //    ViewData["KategoriId"] = new SelectList(_context.Kategoriler, "Id", "Adi");
        //    return View();
        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var kategoriler = _kategoriService.Query().ToList();
            ViewBag.Kategoriler = new SelectList(kategoriler, "Id", "Adi");
            return View();
        }

        // POST: Urunler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Create(Urun urun)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(urun);
        //        _context.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["KategoriId"] = new SelectList(_context.Kategoriler, "Id", "Adi", urun.KategoriId);
        //    return View(urun);
        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Create(UrunModel urun, IFormFile imaj)
        {
            if (ModelState.IsValid)
            {
                #region Dosya Validasyonu
                string fileName = null;
                string fileExtension = null;
                string filePath = null; // sunucuda dosyayı kaydedeceğim yol
                bool saveFile = false; // flag
                if (imaj != null && imaj.Length > 0)
                {
                    fileName = imaj.FileName; // asusrog.jpg
                    fileExtension = Path.GetExtension(fileName); // .jpg
                    string[] appSettingsAcceptedImageExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                    bool acceptedImageExtension = false; // flag
                    foreach (string appSettingsAcceptedImageExtension in appSettingsAcceptedImageExtensions)
                    {
                        if (fileExtension.ToLower() == appSettingsAcceptedImageExtension.ToLower().Trim())
                        {
                            acceptedImageExtension = true;
                            break;
                        }
                    }
                    if (!acceptedImageExtension)
                    {
                        ModelState.AddModelError("", "Dosya uzantısı " + AppSettings.AcceptedImageExtensions + " olmalıdır!");
                        ViewBag.Kategoriler = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi", urun.KategoriId);
                        return View(urun);
                    }
                    // 1 byte = 8 bits
                    // 1 kilobyte = 1024 bytes
                    // 1 megabyte = 1024 kilobytes = 1024 * 1024 bytes
                    double acceptedFileLength = AppSettings.AcceptedImageMaximumLength * Math.Pow(1024, 2);
                    if (imaj.Length > acceptedFileLength)
                    {
                        ModelState.AddModelError("", "Dosya boyutu en çok " + AppSettings.AcceptedImageMaximumLength + " olmalıdır!");
                        ViewBag.Kategoriler = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi", urun.KategoriId);
                        return View(urun);
                    }
                    saveFile = true;
                }
                #endregion

                if (saveFile)
                {
                    fileName = Guid.NewGuid() + fileExtension; // x345f-dert5-gfds2-6hjkl.jpg 
                    filePath = Path.Combine("wwwroot", "files", "urunler", fileName);
                    // ~/wwwroot/files/urunler/x345f-dert5-gfds2-6hjkl.jpg : sanal (virtual path)
                    // C:\Users\Administrator\Source\Repos\ETicaretCoreBilgeAdam8135\MvcWebUI\wwwroot\files\urunler\x345f-dert5-gfds2-6hjkl.jpg : fiziksel (absolute) path
                }

                urun.ImajDosyaAdi = fileName;

                urun.CreatedBy = User.Identity.Name;
                var result = _urunService.Add(urun);
                if (result.IsSuccessful)
                {
                    if (saveFile)
                    {
                        using(FileStream fileStream = new FileStream(filePath, FileMode.CreateNew))
                        {
                            imaj.CopyTo(fileStream);
                        }
                    }
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            var kategoriler = _kategoriService.Query().ToList();
            ViewBag.Kategoriler = new SelectList(kategoriler, "Id", "Adi", urun.KategoriId);
            return View(urun);
        }

        // GET: Urunler/Edit/5
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var urun = _context.Urunler.Find(id);
        //    if (urun == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["KategoriId"] = new SelectList(_context.Kategoriler, "Id", "Adi", urun.KategoriId);
        //    return View(urun);
        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Hata", "Id gereklidir!");
            }
            var urun = _urunService.Query().SingleOrDefault(u => u.Id == id.Value);
            if (urun == null)
            {
                return View("Hata", "Ürün bulunamadı!");
            }
            var kategoriler = _kategoriService.Query().ToList();
            ViewBag.Kategoriler = new SelectList(kategoriler, "Id", "Adi", urun.KategoriId);
            return View(urun);
        }

        // POST: Urunler/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Edit(Urun urun)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Update(urun);
        //        _context.SaveChanges();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["KategoriId"] = new SelectList(_context.Kategoriler, "Id", "Adi", urun.KategoriId);
        //    return View(urun);
        //}
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(UrunModel urun, IFormFile imaj)
        {
            if (ModelState.IsValid)
            {
                #region Dosya Validasyonu
                string fileName = null;
                string fileExtension = null;
                string filePath = null; // sunucuda dosyayı kaydedeceğim yol
                bool saveFile = false; // flag
                if (imaj != null && imaj.Length > 0)
                {
                    fileName = imaj.FileName; // asusrog.jpg
                    fileExtension = Path.GetExtension(fileName); // .jpg
                    string[] appSettingsAcceptedImageExtensions = AppSettings.AcceptedImageExtensions.Split(',');
                    bool acceptedImageExtension = false; // flag
                    foreach (string appSettingsAcceptedImageExtension in appSettingsAcceptedImageExtensions)
                    {
                        if (fileExtension.ToLower() == appSettingsAcceptedImageExtension.ToLower().Trim())
                        {
                            acceptedImageExtension = true;
                            break;
                        }
                    }
                    if (!acceptedImageExtension)
                    {
                        ModelState.AddModelError("", "Dosya uzantısı " + AppSettings.AcceptedImageExtensions + " olmalıdır!");
                        ViewBag.Kategoriler = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi", urun.KategoriId);
                        return View(urun);
                    }
                    // 1 byte = 8 bits
                    // 1 kilobyte = 1024 bytes
                    // 1 megabyte = 1024 kilobytes = 1024 * 1024 bytes
                    double acceptedFileLength = AppSettings.AcceptedImageMaximumLength * Math.Pow(1024, 2);
                    if (imaj.Length > acceptedFileLength)
                    {
                        ModelState.AddModelError("", "Dosya boyutu en çok " + AppSettings.AcceptedImageMaximumLength + " olmalıdır!");
                        ViewBag.Kategoriler = new SelectList(_kategoriService.Query().ToList(), "Id", "Adi", urun.KategoriId);
                        return View(urun);
                    }
                    saveFile = true;
                }
                #endregion

                #region Yeni ürün eklemeden farklı olan kısım
                var existingProduct = _urunService.Query().SingleOrDefault(u => u.Id == urun.Id);

                if (saveFile) // kullanıcı dosya seçtiyse
                {
                    if (string.IsNullOrWhiteSpace(existingProduct.ImajDosyaAdi)) // veritabanında bu ürün için daha önce dosya kaydedilmemişse
                    {
                        fileName = Guid.NewGuid() + fileExtension; // yeni dosya adı ile kullanıcının yüklediği dosyanın uzantısını kullan
                    }
                    else // veritabanında bu ürün için daha önce dosya kaydedilmişse kullanıcının yüklediği dosya uzantısını al ve mevcut dosya adını koru
                    {
                        // x345f-dert5-gfds2-6hjkl.jpg
                        int periodIndex = existingProduct.ImajDosyaAdi.IndexOf("."); // 23
                        fileName = existingProduct.ImajDosyaAdi.Substring(0, periodIndex); // x345f-dert5-gfds2-6hjkl
                        string existingProductImageFileExtension = existingProduct.ImajDosyaAdi.Substring(periodIndex); // .jpg
                        if (fileExtension != existingProductImageFileExtension) // mevcut dosya uzantısı ile yeni dosya uzantısı farklıysa
                        {
                            // sunucudaki mevcut dosya uzantısına sahip dosyayı sil
                            filePath = Path.Combine("wwwroot", "files", "urunler", existingProduct.ImajDosyaAdi);
                            if (System.IO.File.Exists(filePath))
                                System.IO.File.Delete(filePath);
                        }
                        fileName = fileName + fileExtension; // x345f-dert5-gfds2-6hjkl.png
                    }
                }
                else // kullanıcı dosya seçmediyse mevcut dosya adını koru
                {
                    fileName = existingProduct.ImajDosyaAdi;
                }
                #endregion

                urun.ImajDosyaAdi = fileName;

                urun.UpdatedBy = User.Identity.Name;
                var result = _urunService.Update(urun);
                if (result.IsSuccessful)
                {
                    if (saveFile)
                    {
                        filePath = Path.Combine("wwwroot", "files", "urunler", fileName);
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            imaj.CopyTo(fileStream);
                        }
                    }
                    TempData["Message"] = result.Message;
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", result.Message);
            }
            var kategoriler = _kategoriService.Query().ToList();
            ViewBag.Kategoriler = new SelectList(kategoriler, "Id", "Adi", urun.KategoriId);
            return View(urun);
        }

        // GET: Urunler/Delete/5
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var urun = _context.Urunler
        //        .Include(u => u.Kategori)
        //        .SingleOrDefault(m => m.Id == id);
        //    if (urun == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(urun);
        //}
        //[Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (!(User.Identity.IsAuthenticated && User.IsInRole("Admin")))
                return RedirectToAction("YetkisizIslem", "Hesaplar");

            if (id == null)
            {
                return View("Hata", "Id gereklidir!");
            }

            var existingProduct = _urunService.Query().SingleOrDefault(u => u.Id == id.Value);
           
            var result = _urunService.Delete(id.Value);

            if (!string.IsNullOrWhiteSpace(existingProduct.ImajDosyaAdi))
            {
                string filePath = Path.Combine("wwwroot", "files", "urunler", existingProduct.ImajDosyaAdi);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        // POST: Urunler/Delete
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var urun = _context.Urunler.Find(id);
        //    _context.Urunler.Remove(urun);
        //    _context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteUrunImaj(int? id)
        {
            if (id == null)
                return View("Hata", "Id gereklidir!");

            var existingProduct = _urunService.Query().SingleOrDefault(u => u.Id == id.Value);
            if (!string.IsNullOrWhiteSpace(existingProduct.ImajDosyaAdi))
            {
                string filePath = Path.Combine("wwwroot", "files", "urunler", existingProduct.ImajDosyaAdi);
                existingProduct.ImajDosyaAdi = null;
                var result = _urunService.Update(existingProduct);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }

            return View(nameof(Details), existingProduct);
        }
    }
}
