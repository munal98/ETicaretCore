using AppCore.Business.Models.Ordering;
using AppCore.Business.Models.Paging;
using Business.Models.Filters;
using Business.Services.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MvcWebUI.Models;
using MvcWebUI.Settings;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UrunRaporuAjaxController : Controller
    {
        private readonly IUrunService _urunService;

        public UrunRaporuAjaxController(IUrunService urunService)
        {
            _urunService = urunService;
        }

        public IActionResult Index(int? kategoriId)
        {
            var filter = new UrunRaporuFiltreModel()
            {
                KategoriId = kategoriId
            };
            var page = new PageModel() // PagedList kütüphanesi kullanılabilir NuGet'ten
            {
                RecordsPerPageCount = AppSettings.RecordsPerPageCount
            };
            var order = new OrderModel()
            {
                Expression = "Kategori Adı",
                DirectionAscending = true
            };
            var result = _urunService.UrunRaporuGetir(filter, page, order);
            double recordsCount = page.RecordsCount;
            double recordsPerPageCount = page.RecordsPerPageCount;
            double totalPageCount = Math.Ceiling(recordsCount / recordsPerPageCount);
            List<SelectListItem> pageSelectListItems = new List<SelectListItem>();
            if (totalPageCount == 0)
            {
                pageSelectListItems.Add(new SelectListItem()
                {
                    Value = "1",
                    Text = "1"
                });
            }
            else
            {
                for (int pageNumber = 1; pageNumber <= totalPageCount; pageNumber++)
                {
                    pageSelectListItems.Add(new SelectListItem()
                    {
                        Value = pageNumber.ToString(),
                        Text = pageNumber.ToString()
                    });
                }
            }

            var viewModel = new UrunRaporuAjaxIndexViewModel()
            {
                Urunler = result.Data,
                Filtre = filter,
                Sayfalar = new SelectList(pageSelectListItems, "Value", "Text"),
                Sayfa = page,
                Sira = order
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(UrunRaporuAjaxIndexViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // sayfalama
                var page = new PageModel()
                {
                    PageNumber = viewModel.Sayfa.PageNumber,
                    RecordsPerPageCount = AppSettings.RecordsPerPageCount
                };
                var result = _urunService.UrunRaporuGetir(viewModel.Filtre, page, viewModel.Sira);
                viewModel.Urunler = result.Data;

                double recordsCount = page.RecordsCount;
                double recordsPerPageCount = page.RecordsPerPageCount;
                double totalPageCount = Math.Ceiling(recordsCount / recordsPerPageCount);
                List<SelectListItem> pageSelectListItems = new List<SelectListItem>();
                if (totalPageCount == 0)
                {
                    pageSelectListItems.Add(new SelectListItem()
                    {
                        Value = "1",
                        Text = "1"
                    });
                }
                else
                {
                    for (int pageNumber = 1; pageNumber <= totalPageCount; pageNumber++)
                    {
                        pageSelectListItems.Add(new SelectListItem()
                        {
                            Value = pageNumber.ToString(),
                            Text = pageNumber.ToString()
                        });
                    }
                }
                viewModel.Sayfalar = new SelectList(pageSelectListItems, "Value", "Text", page.PageNumber);
                viewModel.Sayfa = page;
            }
           
            return PartialView("_UrunRaporu", viewModel);
        }
    }
}
