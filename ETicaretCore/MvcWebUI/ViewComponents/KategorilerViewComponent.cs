using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace MvcWebUI.ViewComponents
{
    public class KategorilerViewComponent : ViewComponent
    {
        private readonly IKategoriService _kategoriService;

        public KategorilerViewComponent(IKategoriService kategoriService)
        {
            _kategoriService = kategoriService;
        }

        public ViewViewComponentResult Invoke()
        {
            var kategoriler = _kategoriService.Query().ToList();
            return View(kategoriler);
        }
    }
}
